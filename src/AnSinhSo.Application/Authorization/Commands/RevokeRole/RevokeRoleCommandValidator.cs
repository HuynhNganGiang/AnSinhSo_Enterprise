using FluentValidation;

namespace AnSinhSo.Application.Authorization.Commands.RevokeRole;

public sealed class RevokeRoleCommandValidator : AbstractValidator<RevokeRoleCommand>
{
    public RevokeRoleCommandValidator()
    {
        RuleFor(x => x.CitizenIdentityId).NotEmpty();
        RuleFor(x => x.RoleId).NotEmpty();
    }
}
