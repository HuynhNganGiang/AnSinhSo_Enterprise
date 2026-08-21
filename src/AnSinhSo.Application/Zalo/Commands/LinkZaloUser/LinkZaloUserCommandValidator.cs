using FluentValidation;

namespace AnSinhSo.Application.Zalo.Commands.LinkZaloUser;

public sealed class LinkZaloUserCommandValidator : AbstractValidator<LinkZaloUserCommand>
{
    public LinkZaloUserCommandValidator()
    {
        RuleFor(x => x.ZaloUserId).NotEmpty();
        RuleFor(x => x.CitizenIdentityId).NotEmpty();
    }
}
