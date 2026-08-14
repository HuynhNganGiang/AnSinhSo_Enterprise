using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Events.PaymentEvents;

public sealed record PaymentCreatedDomainEvent(Aggregates.PaymentAggregate.PaymentId PaymentId) : DomainEvent;
public sealed record PaymentApprovedDomainEvent(Aggregates.PaymentAggregate.PaymentId PaymentId) : DomainEvent;
public sealed record PaymentCompletedDomainEvent(Aggregates.PaymentAggregate.PaymentId PaymentId) : DomainEvent;
public sealed record PaymentFailedDomainEvent(Aggregates.PaymentAggregate.PaymentId PaymentId) : DomainEvent;
public sealed record PaymentCancelledDomainEvent(Aggregates.PaymentAggregate.PaymentId PaymentId) : DomainEvent;
