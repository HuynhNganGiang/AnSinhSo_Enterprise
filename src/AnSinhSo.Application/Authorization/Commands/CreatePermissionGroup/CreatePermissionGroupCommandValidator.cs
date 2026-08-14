using FluentValidation;

namespace AnSinhSo.Application.Authorization.Commands.CreatePermissionGroup;

public sealed class CreatePermissionGroupCommandValidator : AbstractValidator<CreatePermissionGroupCommand>
{
    public CreatePermissionGroupCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);
            
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
            
        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
