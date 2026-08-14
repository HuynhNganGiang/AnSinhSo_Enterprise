using System.Linq;
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
        var emailResult = Email.Create(request.Email);
        var fullNameResult = FullName.Create("N/A", "N/A", request.FullName);
        var gender = AnSinhSo.Domain.Enumerations.Enumeration.GetAll<AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations.Gender>().FirstOrDefault(x => x.Id == request.Gender) ?? AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations.Gender.Other;

        if (phoneResult.IsFailure) return phoneResult;
        if (addressResult.IsFailure) return addressResult;
        if (emailResult.IsFailure) return emailResult;
        if (fullNameResult.IsFailure) return fullNameResult;

        if (citizen.PhoneNumber.Value != phoneResult.Value.Value && await _citizenRepository.ExistsByPhoneAsync(phoneResult.Value.Value, cancellationToken))
        {
            return Result.Failure(Error.Conflict("Citizen.DuplicatePhone", "Số điện thoại đã tồn tại."));
        }

        if (citizen.Email.Value != emailResult.Value.Value && await _citizenRepository.ExistsByEmailAsync(emailResult.Value.Value, cancellationToken))
        {
            return Result.Failure(Error.Conflict("Citizen.DuplicateEmail", "Địa chỉ email đã tồn tại."));
        }

        citizen.UpdateProfile(fullNameResult.Value, request.BirthDate, gender);
        citizen.ChangePhone(phoneResult.Value);
        citizen.ChangeAddress(addressResult.Value);
        citizen.ChangeEmail(emailResult.Value);

        _citizenRepository.Update(citizen);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
