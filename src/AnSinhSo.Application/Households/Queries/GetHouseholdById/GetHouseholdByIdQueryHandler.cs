using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Application.Households.DTOs;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using System.Linq;
using System.Collections.Generic;

namespace AnSinhSo.Application.Households.Queries.GetHouseholdById;

/// <summary>
/// Handler cho GetHouseholdByIdQuery.
/// </summary>
public sealed class GetHouseholdByIdQueryHandler : IRequestHandler<GetHouseholdByIdQuery, Result<HouseholdDto>>
{
    private readonly IHouseholdRepository _householdRepository;
    private readonly ICitizenRepository _citizenRepository;

    /// <summary>
    /// Khởi tạo GetHouseholdByIdQueryHandler.
    /// </summary>
    public GetHouseholdByIdQueryHandler(
        IHouseholdRepository householdRepository,
        ICitizenRepository citizenRepository)
    {
        _householdRepository = householdRepository;
        _citizenRepository = citizenRepository;
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

        var memberDtos = new List<HouseholdMemberDto>();
        Guid? headCitizenId = null;
        string headCitizenName = string.Empty;

        foreach (var member in household.Members)
        {
            var citizen = await _citizenRepository.GetByIdAsync(member.CitizenId, cancellationToken);
            var citizenName = citizen?.FullName?.ToString() ?? "N/A";
            
            memberDtos.Add(new HouseholdMemberDto(
                member.Id.Value,
                member.CitizenId.Value,
                citizenName,
                member.IsHead,
                System.DateTime.UtcNow)); // TODO: Add JoinedDate to domain model

            if (member.IsHead)
            {
                headCitizenId = member.CitizenId.Value;
                headCitizenName = citizenName;
            }
        }

        var dto = new HouseholdDto(
            household.Id.Value,
            household.HouseholdCode.Value,
            headCitizenId,
            headCitizenName,
            household.Address?.ToString() ?? "",
            household.Status.Id,
            memberDtos
        );

        return Result.Success(dto);
    }
}
