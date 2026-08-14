using FluentValidation;

namespace AnSinhSo.Application.Authorization.Commands.CreateRole;

public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
            
        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
