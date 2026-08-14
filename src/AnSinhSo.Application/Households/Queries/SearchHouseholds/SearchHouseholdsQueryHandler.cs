using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Application.Households.DTOs;
using AnSinhSo.Contracts.Common;
using System.Linq;

namespace AnSinhSo.Application.Households.Queries.SearchHouseholds;

/// <summary>
/// Handler cho SearchHouseholdsQuery.
/// </summary>
public sealed class SearchHouseholdsQueryHandler : IRequestHandler<SearchHouseholdsQuery, Result<PagedResult<HouseholdSummaryDto>>>
{
    private readonly IHouseholdRepository _householdRepository;

    /// <summary>
    /// Khởi tạo SearchHouseholdsQueryHandler.
    /// </summary>
    public SearchHouseholdsQueryHandler(IHouseholdRepository householdRepository)
    {
        _householdRepository = householdRepository;
    }

    /// <summary>
    /// Xử lý truy vấn lấy danh sách hộ gia đình.
    /// </summary>
    public async Task<Result<PagedResult<HouseholdSummaryDto>>> Handle(SearchHouseholdsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _householdRepository.SearchAsync(
            request.Keyword,
            request.Status,
            request.Page,
            request.PageSize,
            request.Sort,
            cancellationToken);

        var dtos = items.Select(x => new HouseholdSummaryDto(
            x.Id.Value,
            x.HouseholdCode.Value,
            "N/A", // Chưa lấy tên chủ hộ do thiết kế repository
            x.Status.Id,
            x.Members.Count
        )).ToList();

        return Result.Success(new PagedResult<HouseholdSummaryDto>(dtos, request.Page, request.PageSize, totalCount));
    }
}
