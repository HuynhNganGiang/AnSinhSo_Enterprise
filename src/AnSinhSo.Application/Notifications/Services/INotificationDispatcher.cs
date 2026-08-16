using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.NotificationAggregate;

namespace AnSinhSo.Application.Notifications.Services;

public interface INotificationDispatcher
{
    Task DispatchAsync(Notification notification, CancellationToken cancellationToken = default);
}
