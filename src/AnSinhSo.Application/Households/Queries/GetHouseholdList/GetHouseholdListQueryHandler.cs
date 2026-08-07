using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Application.Households.DTOs;

namespace AnSinhSo.Application.Households.Queries.GetHouseholdList;

/// <summary>
/// Handler cho GetHouseholdListQuery.
/// </summary>
public sealed class GetHouseholdListQueryHandler : IRequestHandler<GetHouseholdListQuery, Result<IReadOnlyList<HouseholdDto>>>
{
    private readonly IHouseholdRepository _householdRepository;

    /// <summary>
    /// Khởi tạo GetHouseholdListQueryHandler.
    /// </summary>
    public GetHouseholdListQueryHandler(IHouseholdRepository householdRepository)
    {
        _householdRepository = householdRepository;
    }

    /// <summary>
    /// Xử lý truy vấn lấy danh sách hộ gia đình.
    /// </summary>
    public Task<Result<IReadOnlyList<HouseholdDto>>> Handle(GetHouseholdListQuery request, CancellationToken cancellationToken)
    {
        // TODO Step 17: Repository.GetPagedAsync()
        // TODO Step 17: Chưa Mapping
        // TODO Step 17: Chưa DTO

        return Task.FromResult(Result.Success<IReadOnlyList<HouseholdDto>>(new List<HouseholdDto>()));
    }
}
