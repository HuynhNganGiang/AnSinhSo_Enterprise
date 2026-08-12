using System;
using AnSinhSo.Domain.Aggregates.PermissionAggregate;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.RoleAggregate.Entities;

public sealed class RolePermission : Entity<Guid>
{
    public RoleId RoleId { get; private set; }
    public PermissionId PermissionId { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private RolePermission() { } // ORM
#pragma warning restore CS8618

    internal RolePermission(RoleId roleId, PermissionId permissionId)
    {
        Id = Guid.NewGuid();
        RoleId = roleId;
        PermissionId = permissionId;
    }
}
