using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations;
using AnSinhSo.Domain.ValueObjects;
using AnSinhSo.Domain.Interfaces;

namespace AnSinhSo.Application.Citizens.Commands.CreateCitizen;

/// <summary>
/// Handler xử lý lệnh tạo mới công dân.
/// </summary>
public sealed class CreateCitizenCommandHandler : IRequestHandler<CreateCitizenCommand, Result<Guid>>
{
    private readonly ICitizenRepository _citizenRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo CreateCitizenCommandHandler.
    /// </summary>
    public CreateCitizenCommandHandler(ICitizenRepository citizenRepository, IUnitOfWork unitOfWork)
    {
        _citizenRepository = citizenRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Xử lý lệnh tạo mới công dân.
    /// </summary>
    public async Task<Result<Guid>> Handle(CreateCitizenCommand request, CancellationToken cancellationToken)
    {
        var id = new CitizenId(Guid.NewGuid());
        var fullNameResult = FullName.Create("N/A", "N/A", request.FullName);
        var citizenNumberResult = CitizenNumber.Create(request.CitizenNumber);
        var genderResult = Result.Success(AnSinhSo.Domain.Enumerations.Enumeration.GetAll<Gender>().FirstOrDefault(x => x.Id == request.Gender) ?? Gender.Other);
        var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber);
        var addressResult = Address.Create(request.Address, "N/A", "N/A", "N/A", PostalCode.Create("00000").Value);
        var emailResult = Email.Create(request.Email);

        if (citizenNumberResult.IsSuccess && await _citizenRepository.ExistsByCitizenNumberAsync(citizenNumberResult.Value.Value, cancellationToken))
        {
            return Result.Failure<Guid>(Error.Conflict("Citizen.DuplicateNumber", "Số Căn cước công dân đã tồn tại."));
        }

        if (phoneNumberResult.IsSuccess && await _citizenRepository.ExistsByPhoneAsync(phoneNumberResult.Value.Value, cancellationToken))
        {
            return Result.Failure<Guid>(Error.Conflict("Citizen.DuplicatePhone", "Số điện thoại đã tồn tại."));
        }

        if (emailResult.IsSuccess && await _citizenRepository.ExistsByEmailAsync(emailResult.Value.Value, cancellationToken))
        {
            return Result.Failure<Guid>(Error.Conflict("Citizen.DuplicateEmail", "Địa chỉ email đã tồn tại."));
        }

        var citizenResult = Citizen.Create(
            id,
            fullNameResult.Value,
            citizenNumberResult.Value,
            request.BirthDate,
            genderResult.Value,
            phoneNumberResult.Value,
            addressResult.Value,
            emailResult.Value);

        if (citizenResult.IsFailure)
        {
            return Result.Failure<Guid>(citizenResult.Error);
        }

        _citizenRepository.Add(citizenResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(citizenResult.Value.Id.Value);
    }
}
