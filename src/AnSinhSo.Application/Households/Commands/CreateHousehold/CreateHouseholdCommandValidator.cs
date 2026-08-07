using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using FluentValidation;

namespace AnSinhSo.Application.Households.Commands.CreateHousehold;

/// <summary>
/// Validator cho CreateHouseholdCommand.
/// </summary>
public sealed class CreateHouseholdCommandValidator : AbstractValidator<CreateHouseholdCommand>
{
    /// <summary>
    /// Khởi tạo một đối tượng CreateHouseholdCommandValidator mới.
    /// </summary>
    public CreateHouseholdCommandValidator()
    {
        RuleFor(x => x.HouseholdCode)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.AddressId)
            .NotEmpty();

        RuleFor(x => x.HeadCitizenId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);
    }
}
