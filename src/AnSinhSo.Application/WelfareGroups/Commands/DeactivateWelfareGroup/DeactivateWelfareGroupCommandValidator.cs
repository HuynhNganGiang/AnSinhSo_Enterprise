using FluentValidation;

namespace AnSinhSo.Application.WelfareGroups.Commands.DeactivateWelfareGroup;

/// <summary>
/// Validator cho DeactivateWelfareGroupCommand.
/// </summary>
public sealed class DeactivateWelfareGroupCommandValidator : AbstractValidator<DeactivateWelfareGroupCommand>
{
    /// <summary>
    /// Khởi tạo DeactivateWelfareGroupCommandValidator.
    /// </summary>
    public DeactivateWelfareGroupCommandValidator()
    {
        RuleFor(x => x.WelfareGroupId)
            .NotEmpty();
    }
}
