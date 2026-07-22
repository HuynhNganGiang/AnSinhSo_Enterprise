using FluentValidation;

namespace AnSinhSo.Application.Citizens.Commands.ChangeCitizenPhone;

/// <summary>
/// Validator cho ChangeCitizenPhoneCommand.
/// </summary>
public sealed class ChangeCitizenPhoneCommandValidator : AbstractValidator<ChangeCitizenPhoneCommand>
{
    /// <summary>
    /// Khởi tạo ChangeCitizenPhoneCommandValidator.
    /// </summary>
    public ChangeCitizenPhoneCommandValidator()
    {
        RuleFor(x => x.CitizenId)
            .NotEmpty();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty();
    }
}
