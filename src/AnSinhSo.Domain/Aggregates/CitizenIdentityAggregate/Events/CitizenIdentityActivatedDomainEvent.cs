using System;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Events;

public sealed record CitizenIdentityActivatedDomainEvent : IDomainEvent
{
    public CitizenIdentityId CitizenIdentityId { get; }
    public CitizenId CitizenId { get; }
    public DateTime ActivatedAt { get; }

    public CitizenIdentityActivatedDomainEvent(CitizenIdentityId citizenIdentityId, CitizenId citizenId, DateTime activatedAt)
    {
        CitizenIdentityId = citizenIdentityId;
        CitizenId = citizenId;
        ActivatedAt = activatedAt;
    }
}
