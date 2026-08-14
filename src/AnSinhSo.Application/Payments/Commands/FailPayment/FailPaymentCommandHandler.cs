using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Contracts.Common;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Commands.FailPayment;

public class FailPaymentCommandHandler : IRequestHandler<FailPaymentCommand, Result>
{
    private readonly IPaymentRepository _paymentRepository;

    public FailPaymentCommandHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Result> Handle(FailPaymentCommand request, CancellationToken cancellationToken)
    {
        var paymentId = new PaymentId(request.PaymentId);
        var payment = await _paymentRepository.GetByIdAsync(paymentId, cancellationToken);
        if (payment is null)
            return Result.Failure(Error.NotFound("Payment.NotFound", "Payment not found."));

        if (payment.Status == PaymentStatus.Approved)
        {
            var processResult = payment.StartProcessing();
            if (processResult.IsFailure) return processResult;
        }

        var failResult = payment.Fail(request.Reason);
        if (failResult.IsFailure)
            return failResult;

        return Result.Success();
    }
}
