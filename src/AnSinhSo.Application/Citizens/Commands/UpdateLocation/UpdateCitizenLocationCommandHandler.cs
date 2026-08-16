using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.ValueObjects;
using MediatR;

namespace AnSinhSo.Application.Citizens.Commands.UpdateLocation;

internal sealed class UpdateCitizenLocationCommandHandler : IRequestHandler<UpdateCitizenLocationCommand, Result>
{
    private readonly ICitizenRepository _citizenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCitizenLocationCommandHandler(
        ICitizenRepository citizenRepository,
        IUnitOfWork unitOfWork)
    {
        _citizenRepository = citizenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCitizenLocationCommand request, CancellationToken cancellationToken)
    {
        var citizenId = new CitizenId(request.CitizenId);
        var citizen = await _citizenRepository.GetByIdAsync(citizenId, cancellationToken);
        if (citizen is null)
        {
            return Result.Failure(Error.NotFound("Citizen.NotFound", "Không tìm thấy công dân."));
        }

        var locationResult = Location.Create(request.Latitude, request.Longitude);
        if (locationResult.IsFailure)
        {
            return Result.Failure(locationResult.Error);
        }

        var updateResult = citizen.UpdateLocation(locationResult.Value);
        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        _citizenRepository.Update(citizen);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
