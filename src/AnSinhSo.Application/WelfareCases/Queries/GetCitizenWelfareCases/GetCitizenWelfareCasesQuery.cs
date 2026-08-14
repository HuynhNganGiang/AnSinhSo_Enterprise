using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.WelfareCases.DTOs;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.WelfareCases.Queries.GetCitizenWelfareCases;

public record GetCitizenWelfareCasesQuery(Guid CitizenId) : IRequest<Result<IReadOnlyList<WelfareCaseDto>>>;

public class GetCitizenWelfareCasesQueryHandler : IRequestHandler<GetCitizenWelfareCasesQuery, Result<IReadOnlyList<WelfareCaseDto>>>
{
    private readonly IWelfareCaseRepository _repository;

    public GetCitizenWelfareCasesQueryHandler(IWelfareCaseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IReadOnlyList<WelfareCaseDto>>> Handle(GetCitizenWelfareCasesQuery request, CancellationToken cancellationToken)
    {
        var citizenId = new CitizenId(request.CitizenId);
        var cases = await _repository.GetByCitizenIdAsync(citizenId, cancellationToken);

        var dtos = cases.Select(welfareCase => new WelfareCaseDto(
            welfareCase.Id.Value,
            welfareCase.CitizenId.Value,
            welfareCase.HouseholdId?.Value,
            welfareCase.ProgramId.Value,
            welfareCase.Status.Name,
            welfareCase.Notes,
            welfareCase.BenefitAmount,
            welfareCase.EffectiveFrom,
            welfareCase.EffectiveTo,
            new CitizenSnapshotDto(
                welfareCase.CitizenSnapshot.CitizenNumber,
                welfareCase.CitizenSnapshot.FullName,
                welfareCase.CitizenSnapshot.DateOfBirth,
                welfareCase.CitizenSnapshot.Gender,
                welfareCase.CitizenSnapshot.HouseholdCode,
                welfareCase.CitizenSnapshot.Address,
                welfareCase.CitizenSnapshot.Phone,
                welfareCase.CitizenSnapshot.CreatedAtSnapshot)
        )).ToList();

        return Result.Success<IReadOnlyList<WelfareCaseDto>>(dtos);
    }
}
