using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.ValueObjects;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Domain.SeedWork.Errors;

namespace AnSinhSo.Application.Citizens.Commands.ChangeCitizenPhone;

/// <summary>
/// Handler xử lý lệnh thay đổi số điện thoại của công dân.
/// </summary>
public sealed class ChangeCitizenPhoneCommandHandler : IRequestHandler<ChangeCitizenPhoneCommand, Result>
{
    private readonly ICitizenRepository _citizenRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo ChangeCitizenPhoneCommandHandler.
    /// </summary>
    public ChangeCitizenPhoneCommandHandler(ICitizenRepository citizenRepository, IUnitOfWork unitOfWork)
    {
        _citizenRepository = citizenRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Xử lý lệnh thay đổi số điện thoại.
    /// </summary>
    public async Task<Result> Handle(ChangeCitizenPhoneCommand request, CancellationToken cancellationToken)
    {
        var citizenId = new CitizenId(request.CitizenId);
        var citizen = await _citizenRepository.GetByIdAsync(citizenId, cancellationToken);

        if (citizen is null)
        {
            return Result.Failure(DomainErrors.NotFound(nameof(Citizen), request.CitizenId));
        }

        var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber);
        if (phoneNumberResult.IsFailure)
        {
            return Result.Failure(phoneNumberResult.Error);
        }

        var result = citizen.ChangePhone(phoneNumberResult.Value);
        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
