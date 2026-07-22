using FluentValidation;

namespace AnSinhSo.Application.Citizens.Commands.CreateCitizen;

/// <summary>
/// Validator cho CreateCitizenCommand.
/// </summary>
public sealed class CreateCitizenCommandValidator : AbstractValidator<CreateCitizenCommand>
{
    /// <summary>
    /// Khởi tạo CreateCitizenCommandValidator.
    /// </summary>
    public CreateCitizenCommandValidator()
    {
        RuleFor(x => x.CitizenNumber)
            .NotEmpty();

        RuleFor(x => x.FullName)
            .NotEmpty();

        RuleFor(x => x.BirthDate)
            .NotEmpty();

        RuleFor(x => x.Gender)
            .NotNull();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty();

        RuleFor(x => x.Address)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty();
    }
}
