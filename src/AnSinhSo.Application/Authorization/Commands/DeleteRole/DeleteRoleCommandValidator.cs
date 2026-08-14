using FluentValidation;

namespace AnSinhSo.Application.Authorization.Commands.DeleteRole;

public sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
