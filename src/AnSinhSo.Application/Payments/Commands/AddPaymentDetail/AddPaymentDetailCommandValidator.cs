using FluentValidation;

namespace AnSinhSo.Application.Payments.Commands.AddPaymentDetail;

public sealed class AddPaymentDetailCommandValidator : AbstractValidator<AddPaymentDetailCommand>
{
    public AddPaymentDetailCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty();

        RuleFor(x => x.CitizenId)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}
