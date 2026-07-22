using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations;
using AnSinhSo.Domain.ValueObjects;
using AnSinhSo.Application.Abstractions.Persistence;

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
        var id = CitizenId.New();
        var fullNameResult = FullName.Create(request.FullName);
        var citizenNumberResult = CitizenNumber.Create(request.CitizenNumber);
        var genderResult = Gender.FromValue(request.Gender);
        var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber);
        var addressResult = Address.Create(request.Address);
        var emailResult = Email.Create(request.Email);

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

        await _citizenRepository.AddAsync(citizenResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(citizenResult.Value.Id.Value);
    }
}
