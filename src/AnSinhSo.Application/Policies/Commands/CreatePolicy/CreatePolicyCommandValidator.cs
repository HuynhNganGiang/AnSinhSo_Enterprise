using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using FluentValidation;

namespace AnSinhSo.Application.Policies.Commands.CreatePolicy;

/// <summary>
/// Validator cho CreatePolicyCommand.
/// </summary>
public sealed class CreatePolicyCommandValidator : AbstractValidator<CreatePolicyCommand>
{
    /// <summary>
    /// Khởi tạo CreatePolicyCommandValidator.
    /// </summary>
    public CreatePolicyCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Amount)
            .NotNull();
    }
}
