using System;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.RoleAggregate.Events;

public sealed record RolePermissionChangedDomainEvent : IDomainEvent
{
    public RoleId RoleId { get; }
    public DateTime ChangedAt { get; }

    public RolePermissionChangedDomainEvent(RoleId roleId, DateTime changedAt)
    {
        RoleId = roleId;
        ChangedAt = changedAt;
    }
}
