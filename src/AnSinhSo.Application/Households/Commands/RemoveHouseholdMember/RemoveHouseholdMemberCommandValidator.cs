using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using FluentValidation;

namespace AnSinhSo.Application.Households.Commands.RemoveHouseholdMember;

/// <summary>
/// Validator cho RemoveHouseholdMemberCommand.
/// </summary>
public sealed class RemoveHouseholdMemberCommandValidator : AbstractValidator<RemoveHouseholdMemberCommand>
{
    /// <summary>
    /// Khởi tạo một đối tượng RemoveHouseholdMemberCommandValidator mới.
    /// </summary>
    public RemoveHouseholdMemberCommandValidator()
    {
        RuleFor(x => x.HouseholdId)
            .NotEmpty();

        RuleFor(x => x.CitizenId)
            .NotEmpty();
    }
}
