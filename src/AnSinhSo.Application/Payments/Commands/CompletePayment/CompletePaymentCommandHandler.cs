using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Contracts.Common;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Commands.CompletePayment;

public class CompletePaymentCommandHandler : IRequestHandler<CompletePaymentCommand, Result>
{
    private readonly IPaymentRepository _paymentRepository;

    public CompletePaymentCommandHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Result> Handle(CompletePaymentCommand request, CancellationToken cancellationToken)
    {
        var paymentId = new PaymentId(request.PaymentId);
        var payment = await _paymentRepository.GetByIdAsync(paymentId, cancellationToken);
        if (payment is null)
            return Result.Failure(Error.NotFound("Payment.NotFound", "Payment not found."));

        // If Approved, we must transition to Processing before Complete
        if (payment.Status == PaymentStatus.Approved)
        {
            var processResult = payment.StartProcessing();
            if (processResult.IsFailure) return processResult;
        }

        var completeResult = payment.Complete(request.ActualPaymentDate);
        if (completeResult.IsFailure)
            return completeResult;

        return Result.Success();
    }
}
