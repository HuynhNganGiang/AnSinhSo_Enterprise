using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Payments.Queries.GetPaymentList;

public sealed class GetPaymentListQueryHandler : IRequestHandler<GetPaymentListQuery, Result>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetPaymentListQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Result> Handle(GetPaymentListQuery request, CancellationToken cancellationToken)
    {
        // TODO Step 17: Repository call and Mapping to DTO
        return await Task.FromResult(Result.Success());
    }
}
