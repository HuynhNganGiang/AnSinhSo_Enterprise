using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Households.Commands.CreateHousehold;

/// <summary>
/// Handler cho CreateHouseholdCommand.
/// </summary>
public sealed class CreateHouseholdCommandHandler : IRequestHandler<CreateHouseholdCommand, Result<Guid>>
{
    private readonly IHouseholdRepository _householdRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo CreateHouseholdCommandHandler.
    /// </summary>
    public CreateHouseholdCommandHandler(
        IHouseholdRepository householdRepository,
        IUnitOfWork unitOfWork)
    {
        _householdRepository = householdRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Xử lý logic tạo mới hộ gia đình.
    /// </summary>
    public async Task<Result<Guid>> Handle(CreateHouseholdCommand request, CancellationToken cancellationToken)
    {
        var householdId = new HouseholdId(Guid.NewGuid());
        var householdResult = Household.Create(householdId, null!);

        if (householdResult.IsFailure)
        {
            return Result.Failure<Guid>(householdResult.Error);
        }

        _householdRepository.Add(householdResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(householdId.Value);
    }
}
