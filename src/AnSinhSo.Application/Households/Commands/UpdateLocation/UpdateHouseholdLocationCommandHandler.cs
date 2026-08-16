using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.ValueObjects;
using MediatR;

namespace AnSinhSo.Application.Households.Commands.UpdateLocation;

internal sealed class UpdateHouseholdLocationCommandHandler : IRequestHandler<UpdateHouseholdLocationCommand, Result>
{
    private readonly IHouseholdRepository _householdRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHouseholdLocationCommandHandler(
        IHouseholdRepository householdRepository,
        IUnitOfWork unitOfWork)
    {
        _householdRepository = householdRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateHouseholdLocationCommand request, CancellationToken cancellationToken)
    {
        var householdId = new HouseholdId(request.HouseholdId);
        var household = await _householdRepository.GetByIdAsync(householdId, cancellationToken);
        if (household is null)
        {
            return Result.Failure(Error.NotFound("Household.NotFound", "Không tìm thấy hộ gia đình."));
        }

        var locationResult = Location.Create(request.Latitude, request.Longitude);
        if (locationResult.IsFailure)
        {
            return Result.Failure(locationResult.Error);
        }

        var updateResult = household.UpdateLocation(locationResult.Value);
        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        _householdRepository.Update(household);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
