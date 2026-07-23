using FluentValidation;

namespace AnSinhSo.Application.Payments.Commands.ClosePayment;

public sealed class ClosePaymentCommandValidator : AbstractValidator<ClosePaymentCommand>
{
    public ClosePaymentCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty();
    }
}
