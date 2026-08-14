using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Payments.DTOs;
using AnSinhSo.Contracts.Common;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Queries.GetPendingPayments;

public class GetPendingPaymentsQueryHandler : IRequestHandler<GetPendingPaymentsQuery, Result<IReadOnlyList<PaymentDto>>>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetPendingPaymentsQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Result<IReadOnlyList<PaymentDto>>> Handle(GetPendingPaymentsQuery request, CancellationToken cancellationToken)
    {
        var items = await _paymentRepository.GetPendingPaymentsAsync(cancellationToken);

        var dtos = items.Select(payment => new PaymentDto(
            payment.Id.Value,
            payment.PaymentNumber,
            payment.CitizenId.Value,
            payment.HouseholdId?.Value,
            payment.WelfareCaseId.Value,
            payment.Amount,
            payment.ScheduledDate,
            payment.ActualPaymentDate,
            payment.Method.Name,
            payment.Status.Name,
            payment.Notes
        )).ToList();

        return Result.Success<IReadOnlyList<PaymentDto>>(dtos);
    }
}
