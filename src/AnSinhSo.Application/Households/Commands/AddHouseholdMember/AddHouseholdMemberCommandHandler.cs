using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.RelationshipTypeAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Households.Commands.AddHouseholdMember;

/// <summary>
/// Handler cho AddHouseholdMemberCommand.
/// </summary>
public sealed class AddHouseholdMemberCommandHandler : IRequestHandler<AddHouseholdMemberCommand, Result>
{
    private readonly IHouseholdRepository _householdRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IRelationshipTypeRepository _relationshipTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo AddHouseholdMemberCommandHandler.
    /// </summary>
    public AddHouseholdMemberCommandHandler(
        IHouseholdRepository householdRepository,
        ICitizenRepository citizenRepository,
        IRelationshipTypeRepository relationshipTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _householdRepository = householdRepository;
        _citizenRepository = citizenRepository;
        _relationshipTypeRepository = relationshipTypeRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Xử lý thêm thành viên.
    /// </summary>
    public async Task<Result> Handle(AddHouseholdMemberCommand request, CancellationToken cancellationToken)
    {
        var householdId = new HouseholdId(request.HouseholdId);
        var household = await _householdRepository.GetByIdAsync(householdId, cancellationToken);

        if (household is null)
        {
            return Result.Failure(Error.NotFound("Household.NotFound", "Không tìm thấy hộ gia đình."));
        }

        var citizenId = new CitizenId(request.CitizenId);
        var citizenExists = await _citizenRepository.ExistsByIdAsync(citizenId, cancellationToken);
        if (!citizenExists)
        {
            return Result.Failure(Error.NotFound("Citizen.NotFound", "Không tìm thấy công dân."));
        }

        var isCitizenInHousehold = await _householdRepository.IsCitizenInAnyHouseholdAsync(citizenId, cancellationToken);
        if (isCitizenInHousehold)
        {
            return Result.Failure(Error.Conflict("Household.CitizenAlreadyInHousehold", "Công dân đã thuộc một hộ gia đình."));
        }

        var relTypeId = new RelationshipTypeId(request.RelationshipTypeId);
        var relTypeExists = await _relationshipTypeRepository.ExistsByIdAsync(relTypeId, cancellationToken);
        if (!relTypeExists)
        {
            return Result.Failure(Error.NotFound("RelationshipType.NotFound", "Không tìm thấy loại quan hệ."));
        }

        var result = household.AddMember(citizenId, relTypeId, isHead: false);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
