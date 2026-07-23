using FluentValidation;

namespace AnSinhSo.Application.Payments.Commands.CancelPayment;

public sealed class CancelPaymentCommandValidator : AbstractValidator<CancelPaymentCommand>
{
    public CancelPaymentCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty();
    }
}
