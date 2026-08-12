using System;
using System.Text.RegularExpressions;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.Exceptions;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.PermissionGroupAggregate;

namespace AnSinhSo.Domain.Aggregates.PermissionAggregate;

public sealed class Permission : AggregateRoot<PermissionId>
{
    private static readonly Regex CodeRegex = new(@"^[a-z]+\.[a-z]+$", RegexOptions.Compiled);

    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public PermissionGroupId PermissionGroupId { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Permission() { } // ORM
#pragma warning restore CS8618

    public static Result<Permission> Create(
        PermissionId id,
        string code,
        string name,
        string description,
        PermissionGroupId permissionGroupId)
    {
        if (string.IsNullOrWhiteSpace(code) || !CodeRegex.IsMatch(code))
        {
            return Result.Failure<Permission>(AuthorizationErrors.InvalidPermissionCode);
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Permission name cannot be empty.", nameof(name));
        }

        return Result.Success(new Permission
        {
            Id = id,
            Code = code,
            Name = name,
            Description = description,
            PermissionGroupId = permissionGroupId
        });
    }

    public Result Rename(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Permission name cannot be empty.", nameof(name));
        }

        Name = name;
        Description = description;
        return Result.Success();
    }

    public Result ChangeGroup(PermissionGroupId groupId)
    {
        PermissionGroupId = groupId;
        return Result.Success();
    }

    public Result ChangeCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || !CodeRegex.IsMatch(code))
        {
            return Result.Failure(AuthorizationErrors.InvalidPermissionCode);
        }
        Code = code;
        return Result.Success();
    }


}
