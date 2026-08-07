using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using FluentValidation;

namespace AnSinhSo.Application.Policies.Commands.DeactivatePolicy;

/// <summary>
/// Validator cho DeactivatePolicyCommand.
/// </summary>
public sealed class DeactivatePolicyCommandValidator : AbstractValidator<DeactivatePolicyCommand>
{
    /// <summary>
    /// Khởi tạo DeactivatePolicyCommandValidator.
    /// </summary>
    public DeactivatePolicyCommandValidator()
    {
        RuleFor(x => x.PolicyId)
            .NotEmpty();
    }
}
