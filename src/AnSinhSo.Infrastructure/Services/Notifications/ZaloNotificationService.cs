using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Notifications.Services;
using AnSinhSo.Domain.Aggregates.NotificationAggregate;
using Microsoft.Extensions.Logging;

namespace AnSinhSo.Infrastructure.Services.Notifications;

public class ZaloNotificationService : IZaloNotificationService
{
    private readonly ILogger<ZaloNotificationService> _logger;

    public ZaloNotificationService(ILogger<ZaloNotificationService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> SendZaloMessageAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Mocking HTTP call to Zalo OA for Notification {Id}. Content: {Content}", 
            notification.Id.Value, notification.Content);

        // Simulate network delay
        await Task.Delay(100, cancellationToken);

        // Return true to simulate success
        return true;
    }
}
