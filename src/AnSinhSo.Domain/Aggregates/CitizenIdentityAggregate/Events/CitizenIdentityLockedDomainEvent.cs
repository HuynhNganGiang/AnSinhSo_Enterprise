using System;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Events;

public sealed record CitizenIdentityLockedDomainEvent : IDomainEvent
{
    public CitizenIdentityId CitizenIdentityId { get; }
    public string Reason { get; }
    public DateTime LockedAt { get; }

    public CitizenIdentityLockedDomainEvent(CitizenIdentityId citizenIdentityId, string reason, DateTime lockedAt)
    {
        CitizenIdentityId = citizenIdentityId;
        Reason = reason;
        LockedAt = lockedAt;
    }
}
