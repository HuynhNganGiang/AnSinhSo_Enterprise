using AnSinhSo.Domain.SeedWork.Events;
using MediatR;

namespace AnSinhSo.Application.Common.Models;

public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; }

    public DomainEventNotification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}
