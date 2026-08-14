using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Audit;
using AnSinhSo.Application.Common.Models;
using AnSinhSo.Domain.Aggregates.CitizenAggregate.Events;
using MediatR;

namespace AnSinhSo.Application.Citizens.EventHandlers;

public class CitizenDomainEventHandlers : 
    INotificationHandler<DomainEventNotification<CitizenCreatedDomainEvent>>,
    INotificationHandler<DomainEventNotification<CitizenUpdatedDomainEvent>>,
    INotificationHandler<DomainEventNotification<CitizenActivatedDomainEvent>>,
    INotificationHandler<DomainEventNotification<CitizenDeactivatedDomainEvent>>,
    INotificationHandler<DomainEventNotification<CitizenEmailChangedDomainEvent>>,
    INotificationHandler<DomainEventNotification<CitizenPhoneChangedDomainEvent>>,
    INotificationHandler<DomainEventNotification<CitizenAddressChangedDomainEvent>>
{
    private readonly IAuditService _auditService;

    public CitizenDomainEventHandlers(IAuditService auditService)
    {
        _auditService = auditService;
    }

    public async Task Handle(DomainEventNotification<CitizenCreatedDomainEvent> notification, CancellationToken cancellationToken)
    {
        await _auditService.LogEventAsync("Citizen.Created", notification.DomainEvent.CitizenId.Value.ToString(), "Citizen created", cancellationToken);
    }

    public async Task Handle(DomainEventNotification<CitizenUpdatedDomainEvent> notification, CancellationToken cancellationToken)
    {
        await _auditService.LogEventAsync("Citizen.Updated", notification.DomainEvent.CitizenId.Value.ToString(), "Citizen profile updated", cancellationToken);
    }

    public async Task Handle(DomainEventNotification<CitizenActivatedDomainEvent> notification, CancellationToken cancellationToken)
    {
        await _auditService.LogEventAsync("Citizen.Activated", notification.DomainEvent.CitizenId.Value.ToString(), "Citizen activated", cancellationToken);
    }

    public async Task Handle(DomainEventNotification<CitizenDeactivatedDomainEvent> notification, CancellationToken cancellationToken)
    {
        await _auditService.LogEventAsync("Citizen.Deactivated", notification.DomainEvent.CitizenId.Value.ToString(), "Citizen deactivated", cancellationToken);
    }

    public async Task Handle(DomainEventNotification<CitizenEmailChangedDomainEvent> notification, CancellationToken cancellationToken)
    {
        await _auditService.LogEventAsync("Citizen.EmailChanged", notification.DomainEvent.CitizenId.Value.ToString(), $"Email changed to {notification.DomainEvent.Email.Value}", cancellationToken);
    }

    public async Task Handle(DomainEventNotification<CitizenPhoneChangedDomainEvent> notification, CancellationToken cancellationToken)
    {
        await _auditService.LogEventAsync("Citizen.PhoneChanged", notification.DomainEvent.CitizenId.Value.ToString(), $"Phone changed to {notification.DomainEvent.PhoneNumber.Value}", cancellationToken);
    }

    public async Task Handle(DomainEventNotification<CitizenAddressChangedDomainEvent> notification, CancellationToken cancellationToken)
    {
        await _auditService.LogEventAsync("Citizen.AddressChanged", notification.DomainEvent.CitizenId.Value.ToString(), "Address changed", cancellationToken);
    }
}
