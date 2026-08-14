using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.ValueObjects;

namespace AnSinhSo.Application.Households.Commands.UpdateHouseholdAddress;

/// <summary>
/// Handler cho UpdateHouseholdAddressCommand.
/// </summary>
public sealed class UpdateHouseholdAddressCommandHandler : IRequestHandler<UpdateHouseholdAddressCommand, Result>
{
    private readonly IHouseholdRepository _householdRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo UpdateHouseholdAddressCommandHandler.
    /// </summary>
    public UpdateHouseholdAddressCommandHandler(
        IHouseholdRepository householdRepository,
        IUnitOfWork unitOfWork)
    {
        _householdRepository = householdRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Xử lý cập nhật địa chỉ hộ gia đình.
    /// </summary>
    public async Task<Result> Handle(UpdateHouseholdAddressCommand request, CancellationToken cancellationToken)
    {
        var householdId = new HouseholdId(request.HouseholdId);
        var household = await _householdRepository.GetByIdAsync(householdId, cancellationToken);

        if (household is null)
        {
            return Result.Failure(Error.NotFound("Household.NotFound", "Không tìm thấy hộ gia đình."));
        }

        var addressResult = Address.Create(request.Address, "N/A", "N/A", "N/A", PostalCode.Create("00000").Value);
        if (addressResult.IsFailure)
        {
            return Result.Failure(addressResult.Error);
        }

        var result = household.ChangeAddress(addressResult.Value);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
