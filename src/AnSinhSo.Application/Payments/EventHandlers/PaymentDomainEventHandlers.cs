using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Audit;
using AnSinhSo.Application.Common.Models;
using AnSinhSo.Domain.Events.PaymentEvents;
using MediatR;

namespace AnSinhSo.Application.Payments.EventHandlers;

public class PaymentDomainEventHandlers : 
    INotificationHandler<DomainEventNotification<PaymentCreatedDomainEvent>>,
    INotificationHandler<DomainEventNotification<PaymentApprovedDomainEvent>>,
    INotificationHandler<DomainEventNotification<PaymentCompletedDomainEvent>>,
    INotificationHandler<DomainEventNotification<PaymentFailedDomainEvent>>,
    INotificationHandler<DomainEventNotification<PaymentCancelledDomainEvent>>
{
    private readonly IAuditService _auditService;

    public PaymentDomainEventHandlers(IAuditService auditService)
    {
        _auditService = auditService;
    }

    public async Task Handle(DomainEventNotification<PaymentCreatedDomainEvent> notification, CancellationToken cancellationToken)
    {
        await _auditService.LogEventAsync("Payment.Created", notification.DomainEvent.PaymentId.Value.ToString(), "Payment created", cancellationToken);
    }

    public async Task Handle(DomainEventNotification<PaymentApprovedDomainEvent> notification, CancellationToken cancellationToken)
    {
        await _auditService.LogEventAsync("Payment.Approved", notification.DomainEvent.PaymentId.Value.ToString(), "Payment approved", cancellationToken);
    }

    public async Task Handle(DomainEventNotification<PaymentCompletedDomainEvent> notification, CancellationToken cancellationToken)
    {
        await _auditService.LogEventAsync("Payment.Completed", notification.DomainEvent.PaymentId.Value.ToString(), "Payment completed", cancellationToken);
    }

    public async Task Handle(DomainEventNotification<PaymentFailedDomainEvent> notification, CancellationToken cancellationToken)
    {
        await _auditService.LogEventAsync("Payment.Failed", notification.DomainEvent.PaymentId.Value.ToString(), "Payment marked as failed", cancellationToken);
    }

    public async Task Handle(DomainEventNotification<PaymentCancelledDomainEvent> notification, CancellationToken cancellationToken)
    {
        await _auditService.LogEventAsync("Payment.Cancelled", notification.DomainEvent.PaymentId.Value.ToString(), "Payment cancelled", cancellationToken);
    }
}
