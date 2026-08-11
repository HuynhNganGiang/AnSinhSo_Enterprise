using FluentValidation;

namespace AnSinhSo.Application.Authentication.Commands.RefreshToken;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh Token không được trống.");
        RuleFor(x => x.IpAddress).NotEmpty().WithMessage("IP Address không được trống.");
        RuleFor(x => x.UserAgent).NotEmpty().WithMessage("User Agent không được trống.");
    }
}
