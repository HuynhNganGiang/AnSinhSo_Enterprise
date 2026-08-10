using System;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Events;

public sealed record ExternalProviderLinkedDomainEvent : IDomainEvent
{
    public CitizenIdentityId CitizenIdentityId { get; }
    public ProviderType ProviderType { get; }
    public DateTime LinkedAt { get; }

    public ExternalProviderLinkedDomainEvent(CitizenIdentityId citizenIdentityId, ProviderType providerType, DateTime linkedAt)
    {
        CitizenIdentityId = citizenIdentityId;
        ProviderType = providerType;
        LinkedAt = linkedAt;
    }
}
