using FluentValidation;

namespace AnSinhSo.Application.Households.Commands.DeactivateHousehold;

/// <summary>
/// Validator cho DeactivateHouseholdCommand.
/// </summary>
public sealed class DeactivateHouseholdCommandValidator : AbstractValidator<DeactivateHouseholdCommand>
{
    /// <summary>
    /// Khởi tạo một đối tượng DeactivateHouseholdCommandValidator mới.
    /// </summary>
    public DeactivateHouseholdCommandValidator()
    {
        RuleFor(x => x.HouseholdId)
            .NotEmpty();
    }
}
