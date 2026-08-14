using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Contracts.Common;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Commands.ApprovePayment;

public class ApprovePaymentCommandHandler : IRequestHandler<ApprovePaymentCommand, Result>
{
    private readonly IPaymentRepository _paymentRepository;

    public ApprovePaymentCommandHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Result> Handle(ApprovePaymentCommand request, CancellationToken cancellationToken)
    {
        var paymentId = new PaymentId(request.PaymentId);
        var payment = await _paymentRepository.GetByIdAsync(paymentId, cancellationToken);
        if (payment is null)
            return Result.Failure(Error.NotFound("Payment.NotFound", "Payment not found."));

        // By requirement, the workflow is Draft -> Pending -> Approved
        // Since we create it as Draft, let's submit it first to transition to Pending
        if (payment.Status == PaymentStatus.Draft)
        {
            var submitResult = payment.Submit();
            if (submitResult.IsFailure) return submitResult;
        }

        var approveResult = payment.Approve();
        if (approveResult.IsFailure)
            return approveResult;

        return Result.Success();
    }
}
