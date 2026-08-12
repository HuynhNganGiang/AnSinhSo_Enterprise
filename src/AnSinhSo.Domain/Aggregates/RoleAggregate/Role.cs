using System;
using System.Collections.Generic;
using System.Linq;
using AnSinhSo.Domain.Aggregates.PermissionAggregate;
using AnSinhSo.Domain.Aggregates.RoleAggregate.Entities;
using AnSinhSo.Domain.Aggregates.RoleAggregate.Events;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.Exceptions;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Domain.Aggregates.RoleAggregate;

public sealed class Role : AggregateRoot<RoleId>
{
    private readonly List<RolePermission> _permissions = new();

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsSystemRole { get; private set; }

    public IReadOnlyCollection<RolePermission> Permissions => _permissions.AsReadOnly();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Role() { } // ORM
#pragma warning restore CS8618

    public static Result<Role> Create(RoleId id, string name, string description, bool isSystemRole = false)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Role name cannot be empty.", nameof(name));
        }

        return Result.Success(new Role
        {
            Id = id,
            Name = name,
            Description = description,
            IsSystemRole = isSystemRole
        });
    }

    public Result Rename(string newName, string newDescription)
    {
        if (IsSystemRole)
        {
            return Result.Failure(AuthorizationErrors.CannotModifySystemRole);
        }

        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("Role name cannot be empty.", nameof(newName));
        }

        Name = newName;
        Description = newDescription;
        return Result.Success();
    }

    public Result MarkAsSystem()
    {
        IsSystemRole = true;
        return Result.Success();
    }

    public Result MarkAsCustom()
    {
        IsSystemRole = false;
        return Result.Success();
    }

    public Result AddPermission(PermissionId permissionId)
    {
        if (IsSystemRole)
        {
            return Result.Failure(AuthorizationErrors.CannotModifySystemRole);
        }

        if (_permissions.Any(p => p.PermissionId == permissionId))
        {
            return Result.Success(); // Idempotent/Ignore duplicate
        }

        _permissions.Add(new RolePermission(Id, permissionId));
        AddDomainEvent(new RolePermissionChangedDomainEvent(Id, DateTime.UtcNow));
        return Result.Success();
    }

    public Result RemovePermission(PermissionId permissionId)
    {
        if (IsSystemRole)
        {
            return Result.Failure(AuthorizationErrors.CannotModifySystemRole);
        }

        var permissionToRemove = _permissions.FirstOrDefault(p => p.PermissionId == permissionId);
        if (permissionToRemove == null)
        {
            return Result.Success(); // Idempotent
        }

        _permissions.Remove(permissionToRemove);
        AddDomainEvent(new RolePermissionChangedDomainEvent(Id, DateTime.UtcNow));
        return Result.Success();
    }
}
