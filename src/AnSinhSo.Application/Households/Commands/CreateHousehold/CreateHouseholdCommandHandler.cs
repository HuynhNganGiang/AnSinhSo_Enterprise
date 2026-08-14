using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.RelationshipTypeAggregate;
using AnSinhSo.Domain.ValueObjects;

namespace AnSinhSo.Application.Households.Commands.CreateHousehold;

/// <summary>
/// Handler cho CreateHouseholdCommand.
/// </summary>
public sealed class CreateHouseholdCommandHandler : IRequestHandler<CreateHouseholdCommand, Result<Guid>>
{
    private readonly IHouseholdRepository _householdRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IRelationshipTypeRepository _relationshipTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo CreateHouseholdCommandHandler.
    /// </summary>
    public CreateHouseholdCommandHandler(
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
    /// Xử lý logic tạo mới hộ gia đình.
    /// </summary>
    public async Task<Result<Guid>> Handle(CreateHouseholdCommand request, CancellationToken cancellationToken)
    {
        var citizenId = new CitizenId(request.HeadCitizenId);
        
        var citizenExists = await _citizenRepository.ExistsByIdAsync(citizenId, cancellationToken);
        if (!citizenExists)
        {
            return Result.Failure<Guid>(Error.NotFound("Citizen.NotFound", "Không tìm thấy công dân chủ hộ."));
        }

        var isCitizenInHousehold = await _householdRepository.IsCitizenInAnyHouseholdAsync(citizenId, cancellationToken);
        if (isCitizenInHousehold)
        {
            return Result.Failure<Guid>(Error.Conflict("Household.CitizenAlreadyInHousehold", "Công dân đã thuộc một hộ gia đình khác."));
        }

        var relTypeId = new RelationshipTypeId(request.HeadRelationshipTypeId);
        var relTypeExists = await _relationshipTypeRepository.ExistsByIdAsync(relTypeId, cancellationToken);
        if (!relTypeExists)
        {
            return Result.Failure<Guid>(Error.NotFound("RelationshipType.NotFound", "Không tìm thấy loại quan hệ."));
        }

        var householdCode = new HouseholdCode(request.HouseholdCode);
        var codeExists = await _householdRepository.ExistsByCodeAsync(householdCode, cancellationToken);
        if (codeExists)
        {
            return Result.Failure<Guid>(Error.Conflict("Household.CodeExists", "Mã hộ khẩu đã tồn tại trong hệ thống."));
        }

        // Tạo ValueObject Address tạm từ chuỗi, hệ thống thực tế có thể tách thành cấu trúc
        var addressResult = Address.Create(request.Address, "N/A", "N/A", "N/A", PostalCode.Create("00000").Value);
        if (addressResult.IsFailure)
        {
            return Result.Failure<Guid>(addressResult.Error);
        }

        var householdId = new HouseholdId(Guid.NewGuid());
        var householdResult = Household.Create(householdId, householdCode, addressResult.Value);

        if (householdResult.IsFailure)
        {
            return Result.Failure<Guid>(householdResult.Error);
        }

        var household = householdResult.Value;
        var addMemberResult = household.AddMember(citizenId, relTypeId, true);
        if (addMemberResult.IsFailure)
        {
            return Result.Failure<Guid>(addMemberResult.Error);
        }

        _householdRepository.Add(household);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(householdId.Value);
    }
}
