using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Households.Commands.ActivateHousehold;

/// <summary>
/// Handler cho ActivateHouseholdCommand.
/// </summary>
public sealed class ActivateHouseholdCommandHandler : IRequestHandler<ActivateHouseholdCommand, Result>
{
    private readonly IHouseholdRepository _householdRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo ActivateHouseholdCommandHandler.
    /// </summary>
    public ActivateHouseholdCommandHandler(
        IHouseholdRepository householdRepository,
        IUnitOfWork unitOfWork)
    {
        _householdRepository = householdRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Xử lý kích hoạt hộ gia đình.
    /// </summary>
    public async Task<Result> Handle(ActivateHouseholdCommand request, CancellationToken cancellationToken)
    {
        var householdId = new HouseholdId(request.HouseholdId);
        var household = await _householdRepository.GetByIdAsync(householdId, cancellationToken);

        if (household is null)
        {
            return Result.Failure(Error.NotFound("Household.NotFound", "Không tìm thấy hộ gia đình."));
        }

        var result = household.Activate();

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
