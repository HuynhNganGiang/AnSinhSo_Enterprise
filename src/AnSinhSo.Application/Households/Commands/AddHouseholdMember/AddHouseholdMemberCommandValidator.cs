using FluentValidation;

namespace AnSinhSo.Application.Households.Commands.AddHouseholdMember;

/// <summary>
/// Validator cho AddHouseholdMemberCommand.
/// </summary>
public sealed class AddHouseholdMemberCommandValidator : AbstractValidator<AddHouseholdMemberCommand>
{
    /// <summary>
    /// Khởi tạo một đối tượng AddHouseholdMemberCommandValidator mới.
    /// </summary>
    public AddHouseholdMemberCommandValidator()
    {
        RuleFor(x => x.HouseholdId)
            .NotEmpty();

        RuleFor(x => x.CitizenId)
            .NotEmpty();
    }
}
