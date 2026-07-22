using FluentValidation;

namespace AnSinhSo.Application.WelfareGroups.Commands.CreateWelfareGroup;

/// <summary>
/// Validator cho CreateWelfareGroupCommand.
/// </summary>
public sealed class CreateWelfareGroupCommandValidator : AbstractValidator<CreateWelfareGroupCommand>
{
    /// <summary>
    /// Khởi tạo CreateWelfareGroupCommandValidator.
    /// </summary>
    public CreateWelfareGroupCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);
    }
}
