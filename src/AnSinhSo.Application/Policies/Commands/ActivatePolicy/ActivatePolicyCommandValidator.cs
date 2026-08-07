using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using FluentValidation;

namespace AnSinhSo.Application.Policies.Commands.ActivatePolicy;

/// <summary>
/// Validator cho ActivatePolicyCommand.
/// </summary>
public sealed class ActivatePolicyCommandValidator : AbstractValidator<ActivatePolicyCommand>
{
    /// <summary>
    /// Khởi tạo ActivatePolicyCommandValidator.
    /// </summary>
    public ActivatePolicyCommandValidator()
    {
        RuleFor(x => x.PolicyId)
            .NotEmpty();
    }
}
