using FluentValidation;

namespace AnSinhSo.Application.Citizens.Commands.ActivateCitizen;

/// <summary>
/// Validator cho ActivateCitizenCommand.
/// </summary>
public sealed class ActivateCitizenCommandValidator : AbstractValidator<ActivateCitizenCommand>
{
    /// <summary>
    /// Khởi tạo ActivateCitizenCommandValidator.
    /// </summary>
    public ActivateCitizenCommandValidator()
    {
        RuleFor(x => x.CitizenId)
            .NotEmpty();
    }
}
