using System;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.Aggregates.UserRoleAggregate.Events;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.Exceptions;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Domain.Aggregates.UserRoleAggregate;

public sealed class UserRole : AggregateRoot<UserRoleId>
{
    public CitizenIdentityId CitizenIdentityId { get; private set; }
    public RoleId RoleId { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private UserRole() { } // ORM
#pragma warning restore CS8618

    public static Result<UserRole> Assign(
        UserRoleId id, 
        CitizenIdentityId citizenIdentityId, 
        RoleId roleId)
    {
        var userRole = new UserRole
        {
            Id = id,
            CitizenIdentityId = citizenIdentityId,
            RoleId = roleId
        };

        userRole.AddDomainEvent(new UserRoleAssignedDomainEvent(id, citizenIdentityId, roleId, DateTime.UtcNow));
        return Result.Success(userRole);
    }

    public Result Revoke()
    {
        AddDomainEvent(new UserRoleRevokedDomainEvent(Id, CitizenIdentityId, RoleId, DateTime.UtcNow));
        return Result.Success();
    }
}
