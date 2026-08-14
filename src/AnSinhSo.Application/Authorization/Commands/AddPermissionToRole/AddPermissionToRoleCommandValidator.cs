using FluentValidation;

namespace AnSinhSo.Application.Authorization.Commands.AddPermissionToRole;

public sealed class AddPermissionToRoleCommandValidator : AbstractValidator<AddPermissionToRoleCommand>
{
    public AddPermissionToRoleCommandValidator()
    {
        RuleFor(x => x.RoleId).NotEmpty();
        RuleFor(x => x.PermissionId).NotEmpty();
    }
}
