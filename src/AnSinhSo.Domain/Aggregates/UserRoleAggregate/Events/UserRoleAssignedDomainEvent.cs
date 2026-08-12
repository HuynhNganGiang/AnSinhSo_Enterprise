using System;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.UserRoleAggregate.Events;

public sealed record UserRoleAssignedDomainEvent : IDomainEvent
{
    public UserRoleId UserRoleId { get; }
    public CitizenIdentityId CitizenIdentityId { get; }
    public RoleId RoleId { get; }
    public DateTime AssignedAt { get; }

    public UserRoleAssignedDomainEvent(UserRoleId userRoleId, CitizenIdentityId citizenIdentityId, RoleId roleId, DateTime assignedAt)
    {
        UserRoleId = userRoleId;
        CitizenIdentityId = citizenIdentityId;
        RoleId = roleId;
        AssignedAt = assignedAt;
    }
}
