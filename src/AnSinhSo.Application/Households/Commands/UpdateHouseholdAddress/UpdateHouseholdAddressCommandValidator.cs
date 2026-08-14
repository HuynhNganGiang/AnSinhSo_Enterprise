using FluentValidation;

namespace AnSinhSo.Application.Households.Commands.UpdateHouseholdAddress;

/// <summary>
/// Validator cho UpdateHouseholdAddressCommand.
/// </summary>
public sealed class UpdateHouseholdAddressCommandValidator : AbstractValidator<UpdateHouseholdAddressCommand>
{
    /// <summary>
    /// Khởi tạo Validator.
    /// </summary>
    public UpdateHouseholdAddressCommandValidator()
    {
        RuleFor(x => x.HouseholdId)
            .NotEmpty();

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(500);
    }
}
