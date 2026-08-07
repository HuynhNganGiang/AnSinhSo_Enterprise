using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using FluentValidation;

namespace AnSinhSo.Application.Households.Commands.ActivateHousehold;

/// <summary>
/// Validator cho ActivateHouseholdCommand.
/// </summary>
public sealed class ActivateHouseholdCommandValidator : AbstractValidator<ActivateHouseholdCommand>
{
    /// <summary>
    /// Khởi tạo một đối tượng ActivateHouseholdCommandValidator mới.
    /// </summary>
    public ActivateHouseholdCommandValidator()
    {
        RuleFor(x => x.HouseholdId)
            .NotEmpty();
    }
}
