using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Contracts.Citizens;
using AnSinhSo.Contracts.Common;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;
using System.Linq;

namespace AnSinhSo.Application.Citizens.Queries.GetCitizens;

public class GetCitizensQueryHandler : IRequestHandler<GetCitizensQuery, Result<PagedResult<CitizenDto>>>
{
    private readonly ICitizenRepository _citizenRepository;

    public GetCitizensQueryHandler(ICitizenRepository citizenRepository)
    {
        _citizenRepository = citizenRepository;
    }

    public async Task<Result<PagedResult<CitizenDto>>> Handle(GetCitizensQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _citizenRepository.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.Sort,
            cancellationToken);

        var dtos = items.Select(x => new CitizenDto(
            x.Id.Value,
            x.FullName?.ToString() ?? string.Empty,
            x.CitizenNumber.Value,
            x.PhoneNumber.Value,
            x.Email.Value,
            x.Status.ToString()
        )).ToList();

        return Result.Success(new PagedResult<CitizenDto>(dtos, request.Page, request.PageSize, totalCount));
    }
}
