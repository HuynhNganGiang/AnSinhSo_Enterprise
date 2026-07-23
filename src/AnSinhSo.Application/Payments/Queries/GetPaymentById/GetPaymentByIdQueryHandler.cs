using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Application.Common.Errors;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Payments.Queries.GetPaymentById;

public sealed class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, Result>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Result> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        var paymentId = new PaymentId(request.PaymentId);
        var payment = await _paymentRepository.GetWithDetailsAsync(paymentId, cancellationToken);

        if (payment is null)
        {
            return Result.Failure(DomainErrors.NotFound(nameof(Payment), request.PaymentId));
        }

        // TODO Step 17: Return DTO

        return Result.Success();
    }
}
