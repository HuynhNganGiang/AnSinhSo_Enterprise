using FluentValidation;

namespace AnSinhSo.Application.CitizenIdentities.Commands.RegisterCitizenIdentity;

public sealed class RegisterCitizenIdentityCommandValidator : AbstractValidator<RegisterCitizenIdentityCommand>
{
    public RegisterCitizenIdentityCommandValidator()
    {
        RuleFor(x => x.CitizenId)
            .NotEmpty().WithMessage("CitizenId is required.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^(0|\+84)(3|5|7|8|9)[0-9]{8}$").WithMessage("Invalid Vietnamese phone number format.");
    }
}
