using FluentValidation;

namespace AnSinhSo.Application.Authorization.Commands.AssignRole;

public sealed class AssignRoleCommandValidator : AbstractValidator<AssignRoleCommand>
{
    public AssignRoleCommandValidator()
    {
        RuleFor(x => x.CitizenIdentityId).NotEmpty();
        RuleFor(x => x.RoleId).NotEmpty();
    }
}
