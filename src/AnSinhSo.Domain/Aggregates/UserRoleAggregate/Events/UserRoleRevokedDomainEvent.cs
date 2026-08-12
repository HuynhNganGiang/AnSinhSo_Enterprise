using System;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.UserRoleAggregate.Events;

public sealed record UserRoleRevokedDomainEvent : IDomainEvent
{
    public UserRoleId UserRoleId { get; }
    public CitizenIdentityId CitizenIdentityId { get; }
    public RoleId RoleId { get; }
    public DateTime RevokedAt { get; }

    public UserRoleRevokedDomainEvent(UserRoleId userRoleId, CitizenIdentityId citizenIdentityId, RoleId roleId, DateTime revokedAt)
    {
        UserRoleId = userRoleId;
        CitizenIdentityId = citizenIdentityId;
        RoleId = roleId;
        RevokedAt = revokedAt;
    }
}
