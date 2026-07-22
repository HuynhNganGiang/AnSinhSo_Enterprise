using FluentValidation;

namespace AnSinhSo.Application.Policies.Commands.UpdatePolicyAmount;

/// <summary>
/// Validator cho UpdatePolicyAmountCommand.
/// </summary>
public sealed class UpdatePolicyAmountCommandValidator : AbstractValidator<UpdatePolicyAmountCommand>
{
    /// <summary>
    /// Khởi tạo UpdatePolicyAmountCommandValidator.
    /// </summary>
    public UpdatePolicyAmountCommandValidator()
    {
        RuleFor(x => x.PolicyId)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .NotNull();
    }
}
