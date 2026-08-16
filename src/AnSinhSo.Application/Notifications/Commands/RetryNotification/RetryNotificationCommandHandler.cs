using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Notifications.Commands.SendNotification;
using MediatR;

namespace AnSinhSo.Application.Notifications.Commands.RetryNotification;

internal sealed class RetryNotificationCommandHandler : IRequestHandler<RetryNotificationCommand>
{
    private readonly ISender _sender;

    public RetryNotificationCommandHandler(ISender sender)
    {
        _sender = sender;
    }

    public async Task Handle(RetryNotificationCommand request, CancellationToken cancellationToken)
    {
        // Re-use SendNotification logic
        await _sender.Send(new SendNotificationCommand(request.NotificationId), cancellationToken);
    }
}
