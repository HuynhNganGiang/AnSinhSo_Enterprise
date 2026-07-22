using FluentValidation;

namespace AnSinhSo.Application.Citizens.Commands.DeactivateCitizen;

/// <summary>
/// Validator cho DeactivateCitizenCommand.
/// </summary>
public sealed class DeactivateCitizenCommandValidator : AbstractValidator<DeactivateCitizenCommand>
{
    /// <summary>
    /// Khởi tạo DeactivateCitizenCommandValidator.
    /// </summary>
    public DeactivateCitizenCommandValidator()
    {
        RuleFor(x => x.CitizenId)
            .NotEmpty();
    }
}
