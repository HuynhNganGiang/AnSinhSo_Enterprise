using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Households.Commands.ChangeHouseholdHead;

/// <summary>
/// Handler cho ChangeHouseholdHeadCommand.
/// </summary>
public sealed class ChangeHouseholdHeadCommandHandler : IRequestHandler<ChangeHouseholdHeadCommand, Result>
{
    private readonly IHouseholdRepository _householdRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo ChangeHouseholdHeadCommandHandler.
    /// </summary>
    public ChangeHouseholdHeadCommandHandler(
        IHouseholdRepository householdRepository,
        IUnitOfWork unitOfWork)
    {
        _householdRepository = householdRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Xử lý thay đổi chủ hộ.
    /// </summary>
    public async Task<Result> Handle(ChangeHouseholdHeadCommand request, CancellationToken cancellationToken)
    {
        var householdId = new HouseholdId(request.HouseholdId);
        var household = await _householdRepository.GetByIdAsync(householdId, cancellationToken);

        if (household is null)
        {
            return Result.Failure(Error.NotFound("Household.NotFound", "Không tìm thấy hộ gia đình."));
        }

        var citizenId = new CitizenId(request.NewHeadCitizenId);
        var result = household.ChangeHead(citizenId);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
