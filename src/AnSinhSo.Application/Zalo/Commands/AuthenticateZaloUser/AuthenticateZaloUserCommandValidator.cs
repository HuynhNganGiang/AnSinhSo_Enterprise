using FluentValidation;

namespace AnSinhSo.Application.Zalo.Commands.AuthenticateZaloUser;

public sealed class AuthenticateZaloUserCommandValidator : AbstractValidator<AuthenticateZaloUserCommand>
{
    public AuthenticateZaloUserCommandValidator()
    {
        RuleFor(x => x.AuthorizationCode).NotEmpty();
    }
}
