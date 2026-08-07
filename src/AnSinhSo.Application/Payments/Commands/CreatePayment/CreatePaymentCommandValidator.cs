using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using FluentValidation;

namespace AnSinhSo.Application.Payments.Commands.CreatePayment;

public sealed class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.PolicyId)
            .NotEmpty();
    }
}
