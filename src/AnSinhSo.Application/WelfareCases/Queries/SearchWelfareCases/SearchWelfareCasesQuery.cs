using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.WelfareCases.DTOs;
using AnSinhSo.Contracts.Common;
using AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.WelfareCases.Queries.SearchWelfareCases;

public record SearchWelfareCasesQuery(
    string? Keyword,
    Guid? ProgramId,
    int? StatusId,
    int Page,
    int PageSize,
    string? Sort) : IRequest<Result<PagedResult<WelfareCaseDto>>>;

public class SearchWelfareCasesQueryHandler : IRequestHandler<SearchWelfareCasesQuery, Result<PagedResult<WelfareCaseDto>>>
{
    private readonly IWelfareCaseRepository _repository;

    public SearchWelfareCasesQueryHandler(IWelfareCaseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedResult<WelfareCaseDto>>> Handle(SearchWelfareCasesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _repository.SearchAsync(
            request.Keyword,
            request.ProgramId,
            request.StatusId,
            request.Page,
            request.PageSize,
            request.Sort,
            cancellationToken);

        var dtos = items.Select(welfareCase => new WelfareCaseDto(
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

        return Result.Success(new PagedResult<WelfareCaseDto>(dtos, request.Page, request.PageSize, totalCount));
    }
}
