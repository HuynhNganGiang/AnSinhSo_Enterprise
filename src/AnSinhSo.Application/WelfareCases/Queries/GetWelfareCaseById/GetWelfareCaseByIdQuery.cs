using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.WelfareCases.DTOs;
using AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.WelfareCases.Queries.GetWelfareCaseById;

public record GetWelfareCaseByIdQuery(Guid WelfareCaseId) : IRequest<Result<WelfareCaseDto>>;

public class GetWelfareCaseByIdQueryHandler : IRequestHandler<GetWelfareCaseByIdQuery, Result<WelfareCaseDto>>
{
    private readonly IWelfareCaseRepository _repository;

    public GetWelfareCaseByIdQueryHandler(IWelfareCaseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<WelfareCaseDto>> Handle(GetWelfareCaseByIdQuery request, CancellationToken cancellationToken)
    {
        var id = new WelfareCaseId(request.WelfareCaseId);
        var welfareCase = await _repository.GetByIdAsync(id, cancellationToken);

        if (welfareCase == null)
        {
            return Result.Failure<WelfareCaseDto>(Error.NotFound("WelfareCase.NotFound", "The specified welfare case was not found."));
        }

        var snapshotDto = new CitizenSnapshotDto(
            welfareCase.CitizenSnapshot.CitizenNumber,
            welfareCase.CitizenSnapshot.FullName,
            welfareCase.CitizenSnapshot.DateOfBirth,
            welfareCase.CitizenSnapshot.Gender,
            welfareCase.CitizenSnapshot.HouseholdCode,
            welfareCase.CitizenSnapshot.Address,
            welfareCase.CitizenSnapshot.Phone,
            welfareCase.CitizenSnapshot.CreatedAtSnapshot);

        var dto = new WelfareCaseDto(
            welfareCase.Id.Value,
            welfareCase.CitizenId.Value,
            welfareCase.HouseholdId?.Value,
            welfareCase.ProgramId.Value,
            welfareCase.Status.Name,
            welfareCase.Notes,
            welfareCase.BenefitAmount,
            welfareCase.EffectiveFrom,
            welfareCase.EffectiveTo,
            snapshotDto);

        return Result.Success(dto);
    }
}
