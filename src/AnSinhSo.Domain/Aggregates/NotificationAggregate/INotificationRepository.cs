using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Domain.Aggregates.NotificationAggregate;

public interface INotificationRepository
{
    Task<Notification?> GetByIdAsync(NotificationId id, CancellationToken cancellationToken = default);
    Task AddAsync(Notification notification, CancellationToken cancellationToken = default);
    Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default);
}
