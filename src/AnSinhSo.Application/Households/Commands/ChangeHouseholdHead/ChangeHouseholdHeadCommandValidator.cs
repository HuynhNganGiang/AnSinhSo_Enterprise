using FluentValidation;

namespace AnSinhSo.Application.Households.Commands.ChangeHouseholdHead;

/// <summary>
/// Validator cho ChangeHouseholdHeadCommand.
/// </summary>
public sealed class ChangeHouseholdHeadCommandValidator : AbstractValidator<ChangeHouseholdHeadCommand>
{
    /// <summary>
    /// Khởi tạo một đối tượng ChangeHouseholdHeadCommandValidator mới.
    /// </summary>
    public ChangeHouseholdHeadCommandValidator()
    {
        RuleFor(x => x.HouseholdId)
            .NotEmpty();

        RuleFor(x => x.NewHeadCitizenId)
            .NotEmpty();
    }
}
