using FluentValidation;

namespace AnSinhSo.Application.Payments.Commands.CreatePayment;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.CitizenId).NotEmpty();
        RuleFor(x => x.WelfareCaseId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.ScheduledDate).NotEmpty();
        RuleFor(x => x.MethodId).InclusiveBetween(1, 3);
    }
}
