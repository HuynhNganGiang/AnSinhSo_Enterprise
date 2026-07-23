using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.ValueObjects;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Application.Common.Errors;

namespace AnSinhSo.Application.Citizens.Commands.ChangeCitizenAddress;

/// <summary>
/// Handler xử lý lệnh thay đổi địa chỉ thường trú của công dân.
/// </summary>
public sealed class ChangeCitizenAddressCommandHandler : IRequestHandler<ChangeCitizenAddressCommand, Result>
{
    private readonly ICitizenRepository _citizenRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo ChangeCitizenAddressCommandHandler.
    /// </summary>
    public ChangeCitizenAddressCommandHandler(ICitizenRepository citizenRepository, IUnitOfWork unitOfWork)
    {
        _citizenRepository = citizenRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Xử lý lệnh thay đổi địa chỉ thường trú.
    /// </summary>
    public async Task<Result> Handle(ChangeCitizenAddressCommand request, CancellationToken cancellationToken)
    {
        var citizenId = new CitizenId(request.CitizenId);
        var citizen = await _citizenRepository.GetByIdAsync(citizenId, cancellationToken);

        if (citizen is null)
        {
            return Result.Failure(DomainErrors.NotFound(nameof(Citizen), request.CitizenId));
        }

        var addressResult = Address.Create(request.Address, "N/A", "N/A", "N/A", PostalCode.Create("00000").Value);
        if (addressResult.IsFailure)
        {
            return Result.Failure(addressResult.Error);
        }

        var result = citizen.ChangeAddress(addressResult.Value);
        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
