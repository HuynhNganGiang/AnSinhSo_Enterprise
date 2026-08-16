using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.NotificationAggregate;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AnSinhSoDbContext _context;

    public NotificationRepository(AnSinhSoDbContext context)
    {
        _context = context;
    }

    public async Task<Notification?> GetByIdAsync(NotificationId id, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .Include(x => x.Timeline)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
