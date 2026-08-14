using FluentValidation;

namespace AnSinhSo.Application.Authorization.Commands.UpdateRole;

public sealed class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
            
        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
