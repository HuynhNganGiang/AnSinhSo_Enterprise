using FluentValidation;

namespace AnSinhSo.Application.WelfareGroups.Commands.ActivateWelfareGroup;

/// <summary>
/// Validator cho ActivateWelfareGroupCommand.
/// </summary>
public sealed class ActivateWelfareGroupCommandValidator : AbstractValidator<ActivateWelfareGroupCommand>
{
    /// <summary>
    /// Khởi tạo ActivateWelfareGroupCommandValidator.
    /// </summary>
    public ActivateWelfareGroupCommandValidator()
    {
        RuleFor(x => x.WelfareGroupId)
            .NotEmpty();
    }
}
