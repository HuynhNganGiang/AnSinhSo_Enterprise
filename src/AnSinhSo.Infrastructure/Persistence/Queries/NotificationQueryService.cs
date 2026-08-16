using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Notifications.DTOs;
using AnSinhSo.Application.Notifications.Queries.GetNotifications;
using AnSinhSo.Application.Notifications.Services;
using AnSinhSo.Domain.Aggregates.NotificationAggregate;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Queries;

public class NotificationQueryService : INotificationQueryService
{
    private readonly AnSinhSoDbContext _context;

    public NotificationQueryService(AnSinhSoDbContext context)
    {
        _context = context;
    }

    public async Task<List<NotificationDto>> GetNotificationsAsync(GetNotificationsQuery query, CancellationToken cancellationToken = default)
    {
        var dbQuery = _context.Notifications.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            dbQuery = dbQuery.Where(n => n.Title.Contains(query.Keyword) || n.Content.Contains(query.Keyword));
        }
        
        if (query.Date.HasValue)
        {
            dbQuery = dbQuery.Where(n => n.CreatedAt.Date == query.Date.Value.Date);
        }

        if (query.Channel.HasValue)
        {
            dbQuery = dbQuery.Where(n => n.Channel == query.Channel.Value);
        }

        if (query.Status.HasValue)
        {
            dbQuery = dbQuery.Where(n => n.Status == query.Status.Value);
        }

        if (query.Priority.HasValue)
        {
            dbQuery = dbQuery.Where(n => n.Priority == query.Priority.Value);
        }

        if (query.SourceModule.HasValue)
        {
            dbQuery = dbQuery.Where(n => n.SourceModule == query.SourceModule.Value);
        }

        var list = await dbQuery.OrderByDescending(x => x.CreatedAt).ToListAsync(cancellationToken);
        return list.Select(MapToDto).ToList();
    }

    public async Task<List<NotificationDto>> GetUnreadNotificationsAsync(CancellationToken cancellationToken = default)
    {
        var list = await _context.Notifications.AsNoTracking()
            .Where(n => n.Status == NotificationStatus.Sent)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
            
        return list.Select(MapToDto).ToList();
    }

    public async Task<NotificationDto> GetNotificationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var notif = await _context.Notifications.AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == NotificationId.Create(id), cancellationToken);
            
        return notif == null ? null! : MapToDto(notif);
    }

    public async Task<NotificationStatisticsDto> GetStatisticsAsync(CancellationToken cancellationToken = default)
    {
        var all = await _context.Notifications.AsNoTracking()
            .GroupBy(x => x.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var stats = new NotificationStatisticsDto();
        foreach (var item in all)
        {
            switch (item.Status)
            {
                case NotificationStatus.Pending:
                    stats.Pending = item.Count;
                    break;
                case NotificationStatus.Sent:
                    // Sent is considered unread
                    stats.Sent = item.Count;
                    stats.Unread = item.Count;
                    break;
                case NotificationStatus.Failed:
                    stats.Failed = item.Count;
                    break;
                case NotificationStatus.Read:
                    stats.Read = item.Count;
                    break;
            }
        }

        return stats;
    }

    public async Task<List<NotificationDto>> GetUserNotificationsAsync(Guid citizenId, CancellationToken cancellationToken = default)
    {
        var list = await _context.Notifications.AsNoTracking()
            .Where(n => n.RecipientCitizenId == new CitizenId(citizenId))
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
            
        return list.Select(MapToDto).ToList();
    }

    public async Task<UnreadCountDto> GetUnreadCountAsync(Guid citizenId, CancellationToken cancellationToken = default)
    {
        var count = await _context.Notifications.AsNoTracking()
            .Where(n => n.RecipientCitizenId == new CitizenId(citizenId) && n.Status == NotificationStatus.Sent)
            .CountAsync(cancellationToken);
            
        return new UnreadCountDto { Count = count };
    }

    private static NotificationDto MapToDto(Notification entity)
    {
        return new NotificationDto
        {
            Id = entity.Id.Value,
            Title = entity.Title,
            Content = entity.Content,
            RecipientCitizenId = entity.RecipientCitizenId.Value,
            Channel = entity.Channel.ToString(),
            Status = entity.Status.ToString(),
            Priority = entity.Priority.ToString(),
            CreatedAt = entity.CreatedAt,
            ScheduledAt = entity.ScheduledAt,
            SentAt = entity.SentAt,
            ReadAt = entity.ReadAt,
            RetryCount = entity.RetryCount,
            SourceModule = entity.SourceModule?.ToString(),
            SourceId = entity.SourceId
        };
    }
}
