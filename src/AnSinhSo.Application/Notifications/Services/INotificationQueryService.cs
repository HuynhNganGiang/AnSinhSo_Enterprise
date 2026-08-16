using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Notifications.DTOs;
using AnSinhSo.Application.Notifications.Queries.GetNotifications;

namespace AnSinhSo.Application.Notifications.Services;

public interface INotificationQueryService
{
    Task<List<NotificationDto>> GetNotificationsAsync(GetNotificationsQuery query, CancellationToken cancellationToken = default);
    Task<List<NotificationDto>> GetUnreadNotificationsAsync(CancellationToken cancellationToken = default);
    Task<NotificationDto> GetNotificationByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<NotificationStatisticsDto> GetStatisticsAsync(CancellationToken cancellationToken = default);
    Task<List<NotificationDto>> GetUserNotificationsAsync(Guid citizenId, CancellationToken cancellationToken = default);
    Task<UnreadCountDto> GetUnreadCountAsync(Guid citizenId, CancellationToken cancellationToken = default);
}
