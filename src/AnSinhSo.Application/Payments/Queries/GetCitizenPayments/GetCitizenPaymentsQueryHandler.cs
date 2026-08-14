using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Payments.DTOs;
using AnSinhSo.Contracts.Common;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Queries.GetCitizenPayments;

public class GetCitizenPaymentsQueryHandler : IRequestHandler<GetCitizenPaymentsQuery, Result<IReadOnlyList<PaymentDto>>>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetCitizenPaymentsQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Result<IReadOnlyList<PaymentDto>>> Handle(GetCitizenPaymentsQuery request, CancellationToken cancellationToken)
    {
        // Simple search for citizen ID with no pagination to get all of them, or just use page 1 size 1000
        var (items, _) = await _paymentRepository.SearchAsync(
            keyword: null,
            citizenId: request.CitizenId,
            householdId: null,
            welfareCaseId: null,
            statusId: null,
            methodId: null,
            fromDate: null,
            toDate: null,
            page: 1,
            pageSize: 1000,
            sort: "createdat_desc",
            cancellationToken);

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
