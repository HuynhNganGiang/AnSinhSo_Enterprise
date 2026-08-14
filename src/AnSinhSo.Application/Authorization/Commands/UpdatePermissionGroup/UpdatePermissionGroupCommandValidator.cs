using FluentValidation;

namespace AnSinhSo.Application.Authorization.Commands.UpdatePermissionGroup;

public sealed class UpdatePermissionGroupCommandValidator : AbstractValidator<UpdatePermissionGroupCommand>
{
    public UpdatePermissionGroupCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
            
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
            
        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
