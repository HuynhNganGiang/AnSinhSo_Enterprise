using FluentValidation;

namespace AnSinhSo.Application.Authentication.BackOffice.BackOfficeLogin;

public sealed class BackOfficeLoginCommandValidator : AbstractValidator<BackOfficeLoginCommand>
{
    public BackOfficeLoginCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
