using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Payments.DTOs;
using AnSinhSo.Contracts.Common;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Queries.SearchPayments;

public class SearchPaymentsQueryHandler : IRequestHandler<SearchPaymentsQuery, Result<PagedResult<PaymentDto>>>
{
    private readonly IPaymentRepository _paymentRepository;

    public SearchPaymentsQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Result<PagedResult<PaymentDto>>> Handle(SearchPaymentsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _paymentRepository.SearchAsync(
            request.Request.Keyword,
            request.Request.CitizenId,
            request.Request.HouseholdId,
            request.Request.WelfareCaseId,
            request.Request.StatusId,
            request.Request.MethodId,
            request.Request.FromDate,
            request.Request.ToDate,
            request.Request.Page,
            request.Request.PageSize,
            request.Request.Sort,
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

        return Result.Success(new PagedResult<PaymentDto>(dtos, request.Request.Page, request.Request.PageSize, totalCount));
    }
}
