using FluentValidation;

namespace AnSinhSo.Application.Authorization.Commands.DeletePermissionGroup;

public sealed class DeletePermissionGroupCommandValidator : AbstractValidator<DeletePermissionGroupCommand>
{
    public DeletePermissionGroupCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
