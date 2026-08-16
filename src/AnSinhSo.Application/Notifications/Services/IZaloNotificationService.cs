using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.NotificationAggregate;

namespace AnSinhSo.Application.Notifications.Services;

public interface IZaloNotificationService
{
    Task<bool> SendZaloMessageAsync(Notification notification, CancellationToken cancellationToken = default);
}
