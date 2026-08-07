using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Households.Commands.AddHouseholdMember;

/// <summary>
/// Handler cho AddHouseholdMemberCommand.
/// </summary>
public sealed class AddHouseholdMemberCommandHandler : IRequestHandler<AddHouseholdMemberCommand, Result>
{
    private readonly IHouseholdRepository _householdRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo AddHouseholdMemberCommandHandler.
    /// </summary>
    public AddHouseholdMemberCommandHandler(
        IHouseholdRepository householdRepository,
        IUnitOfWork unitOfWork)
    {
        _householdRepository = householdRepository;
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
        var result = household.AddMember(citizenId, isHead: false);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
