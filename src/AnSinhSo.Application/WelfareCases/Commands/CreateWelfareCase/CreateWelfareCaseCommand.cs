using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;
using AnSinhSo.Domain.Aggregates.WelfareProgramAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;
using FluentValidation;

namespace AnSinhSo.Application.WelfareCases.Commands.CreateWelfareCase;

public record CreateWelfareCaseCommand(
    Guid CitizenId,
    Guid ProgramId,
    string? Notes) : IRequest<Result<Guid>>;

public class CreateWelfareCaseCommandValidator : AbstractValidator<CreateWelfareCaseCommand>
{
    public CreateWelfareCaseCommandValidator()
    {
        RuleFor(x => x.CitizenId).NotEmpty();
        RuleFor(x => x.ProgramId).NotEmpty();
    }
}

public class CreateWelfareCaseCommandHandler : IRequestHandler<CreateWelfareCaseCommand, Result<Guid>>
{
    private readonly IWelfareCaseRepository _welfareCaseRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IHouseholdRepository _householdRepository;
    private readonly IWelfareProgramRepository _welfareProgramRepository;

    public CreateWelfareCaseCommandHandler(
        IWelfareCaseRepository welfareCaseRepository,
        ICitizenRepository citizenRepository,
        IHouseholdRepository householdRepository,
        IWelfareProgramRepository welfareProgramRepository)
    {
        _welfareCaseRepository = welfareCaseRepository;
        _citizenRepository = citizenRepository;
        _householdRepository = householdRepository;
        _welfareProgramRepository = welfareProgramRepository;
    }

    public async Task<Result<Guid>> Handle(CreateWelfareCaseCommand request, CancellationToken cancellationToken)
    {
        var citizenId = new CitizenId(request.CitizenId);
        var programId = new WelfareProgramId(request.ProgramId);

        var program = await _welfareProgramRepository.GetByIdAsync(programId, cancellationToken);
        if (program == null || !program.IsActive)
        {
            return Result.Failure<Guid>(Error.NotFound("WelfareProgram.NotFound", "The specified welfare program was not found or is inactive."));
        }

        var citizen = await _citizenRepository.GetByIdAsync(citizenId, cancellationToken);
        if (citizen == null)
        {
            return Result.Failure<Guid>(Error.NotFound("Citizen.NotFound", "The specified citizen was not found."));
        }

        // Check active case duplicate
        var existsActiveCase = await _welfareCaseRepository.ExistsActiveCaseForProgramAsync(citizenId, programId, cancellationToken);
        if (existsActiveCase)
        {
            return Result.Failure<Guid>(Error.Conflict("WelfareCase.Duplicate", "The citizen already has an active welfare case for this program."));
        }

        Household? household = await _householdRepository.GetByCitizenIdAsync(citizenId, cancellationToken);
        string? householdCode = household?.HouseholdCode?.Value;

        var snapshot = CitizenSnapshot.Create(
            citizenNumber: citizen.CitizenNumber.Value,
            fullName: $"{citizen.FullName.FirstName} {citizen.FullName.MiddleName} {citizen.FullName.LastName}".Replace("  ", " ").Trim(),
            dateOfBirth: citizen.BirthDate,
            gender: citizen.Gender.Name,
            householdCode: householdCode,
            address: $"{citizen.Address.Street}, {citizen.Address.Ward}, {citizen.Address.District}, {citizen.Address.Province}",
            phone: citizen.PhoneNumber?.Value);

        var welfareCaseId = new WelfareCaseId(Guid.NewGuid());
        var createResult = WelfareCase.Create(
            welfareCaseId,
            citizenId,
            household?.Id,
            programId,
            snapshot);

        if (createResult.IsFailure) return Result.Failure<Guid>(createResult.Error);

        var newCase = createResult.Value;
        if (!string.IsNullOrEmpty(request.Notes))
        {
            newCase.UpdateDetails(request.Notes, null, null, null);
        }

        _welfareCaseRepository.Add(newCase);
        // Unit of Work will be saved through behavior

        return Result.Success(welfareCaseId.Value);
    }
}
