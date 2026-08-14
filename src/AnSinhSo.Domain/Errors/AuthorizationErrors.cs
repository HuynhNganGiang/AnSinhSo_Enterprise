using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Domain.Errors;

public static class AuthorizationErrors
{
    public static readonly Error RoleNotFound = new(
        "Authorization.RoleNotFound",
        "The specified role does not exist.",
        ErrorType.NotFound
    );

    public static readonly Error CannotModifySystemRole = new(
        "Authorization.CannotModifySystemRole",
        "System roles cannot be modified or deleted.",
        ErrorType.Validation
    );

    public static readonly Error DuplicatePermissionCode = new(
        "Authorization.DuplicatePermissionCode",
        "The permission code already exists.",
        ErrorType.Conflict
    );

    public static readonly Error InvalidPermissionCode = new(
        "Authorization.InvalidPermissionCode",
        "Permission code must not be empty and should follow resource.action convention.",
        ErrorType.Validation
    );
    
    public static readonly Error UserRoleAlreadyExists = new(
        "Authorization.UserRoleAlreadyExists",
        "The user is already assigned to this role.",
        ErrorType.Conflict
    );
    
    public static readonly Error UserRoleNotFound = new(
        "Authorization.UserRoleNotFound",
        "The user is not assigned to this role.",
        ErrorType.NotFound
    );
    
    public static readonly Error RoleInUse = new(
        "Authorization.RoleInUse",
        "The role is currently assigned to one or more users and cannot be deleted.",
        ErrorType.Conflict
    );
}
