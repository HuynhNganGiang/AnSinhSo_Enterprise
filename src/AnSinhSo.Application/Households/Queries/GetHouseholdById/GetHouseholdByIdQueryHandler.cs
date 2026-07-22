using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Application.Households.DTOs;

namespace AnSinhSo.Application.Households.Queries.GetHouseholdById;

/// <summary>
/// Handler cho GetHouseholdByIdQuery.
/// </summary>
public sealed class GetHouseholdByIdQueryHandler : IRequestHandler<GetHouseholdByIdQuery, Result<HouseholdDto>>
{
    private readonly IHouseholdRepository _householdRepository;

    /// <summary>
    /// Khởi tạo GetHouseholdByIdQueryHandler.
    /// </summary>
    public GetHouseholdByIdQueryHandler(IHouseholdRepository householdRepository)
    {
        _householdRepository = householdRepository;
    }

    /// <summary>
    /// Xử lý truy vấn lấy thông tin chi tiết hộ gia đình.
    /// </summary>
    public async Task<Result<HouseholdDto>> Handle(GetHouseholdByIdQuery request, CancellationToken cancellationToken)
    {
        var householdId = new HouseholdId(request.HouseholdId);

        // TODO Step 17: Repository.GetByIdAsync()
        var household = await _householdRepository.GetByIdAsync(householdId, cancellationToken);

        if (household is null)
        {
            return Result.Failure<HouseholdDto>(Error.NotFound("Household.NotFound", "Không tìm thấy hộ gia đình."));
        }

        // TODO Step 17: Mapping
        // TODO Step 17: DTO
        return Result.Success<HouseholdDto>(default!);
    }
}
