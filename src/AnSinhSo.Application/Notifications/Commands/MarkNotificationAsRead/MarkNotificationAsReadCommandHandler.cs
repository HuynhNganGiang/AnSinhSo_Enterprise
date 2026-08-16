using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Shared.Exceptions;
using AnSinhSo.Domain.Aggregates.NotificationAggregate;
using MediatR;

namespace AnSinhSo.Application.Notifications.Commands.MarkNotificationAsRead;

internal sealed class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand>
{
    private readonly INotificationRepository _notificationRepository;

    public MarkNotificationAsReadCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.GetByIdAsync(NotificationId.Create(request.NotificationId), cancellationToken);

        if (notification is null)
        {
            throw new NotFoundException("Notification with ID $($request.NotificationId) not found");
        }

        notification.MarkAsRead(DateTime.UtcNow);
        await _notificationRepository.UpdateAsync(notification, cancellationToken);
    }
}
