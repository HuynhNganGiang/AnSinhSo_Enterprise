using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.ValueObjects;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Application.Common.Errors;

namespace AnSinhSo.Application.Citizens.Commands.UpdateCitizen;

public sealed class UpdateCitizenCommandHandler : IRequestHandler<UpdateCitizenCommand, Result>
{
    private readonly ICitizenRepository _citizenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCitizenCommandHandler(ICitizenRepository citizenRepository, IUnitOfWork unitOfWork)
    {
        _citizenRepository = citizenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCitizenCommand request, CancellationToken cancellationToken)
    {
        var citizenId = new CitizenId(request.CitizenId);
        var citizen = await _citizenRepository.GetByIdAsync(citizenId, cancellationToken);

        if (citizen is null)
        {
            return Result.Failure(DomainErrors.NotFound(nameof(Citizen), request.CitizenId));
        }

        var phoneResult = PhoneNumber.Create(request.PhoneNumber);
        var addressResult = Address.Create(request.Address, "N/A", "N/A", "N/A", PostalCode.Create("00000").Value);

        if (phoneResult.IsFailure) return phoneResult;
        if (addressResult.IsFailure) return addressResult;

        var phoneUpdateResult = citizen.ChangePhone(phoneResult.Value);
        if (phoneUpdateResult.IsFailure) return phoneUpdateResult;

        var addressUpdateResult = citizen.ChangeAddress(addressResult.Value);
        if (addressUpdateResult.IsFailure) return addressUpdateResult;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
