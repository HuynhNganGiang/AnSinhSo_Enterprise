using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Shared.Exceptions;
using AnSinhSo.Application.Notifications.Services;
using AnSinhSo.Domain.Aggregates.NotificationAggregate;
using MediatR;

namespace AnSinhSo.Application.Notifications.Commands.SendNotification;

internal sealed class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationDispatcher _notificationDispatcher;

    public SendNotificationCommandHandler(
        INotificationRepository notificationRepository,
        INotificationDispatcher notificationDispatcher)
    {
        _notificationRepository = notificationRepository;
        _notificationDispatcher = notificationDispatcher;
    }

    public async Task Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.GetByIdAsync(NotificationId.Create(request.NotificationId), cancellationToken);

        if (notification is null)
        {
            throw new NotFoundException("Notification with ID $($request.NotificationId) not found");
        }

        if (notification.Status == NotificationStatus.Sent || notification.Status == NotificationStatus.Read)
        {
            // Already sent, do nothing
            return;
        }
        
        if (notification.Status == NotificationStatus.Failed && notification.RetryCount >= 3)
        {
            // Permanently failed
            return;
        }

        notification.MarkAsProcessing();
        await _notificationRepository.UpdateAsync(notification, cancellationToken);

        try
        {
            await _notificationDispatcher.DispatchAsync(notification, cancellationToken);
            notification.MarkAsSent(DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            notification.MarkAsFailed(ex.Message);
        }

        await _notificationRepository.UpdateAsync(notification, cancellationToken);
    }
}
