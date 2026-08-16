using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Notifications.Services;
using AnSinhSo.Domain.Aggregates.NotificationAggregate;
using Microsoft.Extensions.Logging;

namespace AnSinhSo.Infrastructure.Services.Notifications;

public class NotificationDispatcher : INotificationDispatcher
{
    private readonly IZaloNotificationService _zaloService;
    private readonly ILogger<NotificationDispatcher> _logger;

    public NotificationDispatcher(IZaloNotificationService zaloService, ILogger<NotificationDispatcher> logger)
    {
        _zaloService = zaloService;
        _logger = logger;
    }

    public async Task DispatchAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Dispatching notification {Id} via {Channel}", notification.Id.Value, notification.Channel);

        switch (notification.Channel)
        {
            case NotificationChannel.ZaloOA:
                var success = await _zaloService.SendZaloMessageAsync(notification, cancellationToken);
                if (!success)
                {
                    throw new Exception("Zalo OA Service failed to send the message.");
                }
                break;
            case NotificationChannel.InApp:
                // For InApp, typically no external call is made, it just sits in the DB.
                // Alternatively, SignalR could be called here to push real-time updates.
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(notification.Channel), "Unsupported channel.");
        }
    }
}
