using System;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Domain.Aggregates.PermissionGroupAggregate;

public sealed class PermissionGroup : AggregateRoot<PermissionGroupId>
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private PermissionGroup() { } // ORM
#pragma warning restore CS8618

    public static Result<PermissionGroup> Create(
        PermissionGroupId id,
        string code,
        string name,
        string description)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("PermissionGroup code cannot be empty.", nameof(code));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("PermissionGroup name cannot be empty.", nameof(name));
        }

        return Result.Success(new PermissionGroup
        {
            Id = id,
            Code = code,
            Name = name,
            Description = description
        });
    }
    
    public Result Rename(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("PermissionGroup name cannot be empty.", nameof(name));
        }
        
        Name = name;
        Description = description;
        return Result.Success();
    }
}
