using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.NotificationAggregate;
using MediatR;

namespace AnSinhSo.Application.Notifications.Commands.CreateNotification;

internal sealed class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Guid>
{
    private readonly INotificationRepository _notificationRepository;

    public CreateNotificationCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Guid> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = Notification.Create(
            request.Title,
            request.Content,
            new CitizenId(request.RecipientCitizenId),
            request.Channel,
            request.Priority,
            request.ScheduledAt,
            Guid.NewGuid(), // CorrelationId
            request.SourceModule,
            request.SourceId);

        await _notificationRepository.AddAsync(notification, cancellationToken);
        
        return notification.Id.Value;
    }
}
