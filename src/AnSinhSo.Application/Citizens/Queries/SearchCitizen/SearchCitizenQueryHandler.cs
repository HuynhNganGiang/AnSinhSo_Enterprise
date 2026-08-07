using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Citizens.DTOs;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using AutoMapper;
using MediatR;
using System.Linq;

namespace AnSinhSo.Application.Citizens.Queries.SearchCitizen;

public class SearchCitizenQueryHandler : IRequestHandler<SearchCitizenQuery, Result<IReadOnlyList<CitizenSummaryDto>>>
{
    private readonly ICitizenRepository _citizenRepository;
    private readonly IMapper _mapper;

    public SearchCitizenQueryHandler(ICitizenRepository citizenRepository, IMapper mapper)
    {
        _citizenRepository = citizenRepository;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<CitizenSummaryDto>>> Handle(SearchCitizenQuery request, CancellationToken cancellationToken)
    {
        var citizens = await _citizenRepository.ListAsync(new SearchCitizenSpecification(request.Keyword), cancellationToken);

        var dtos = _mapper.Map<List<CitizenSummaryDto>>(citizens.ToList());

        return Result.Success<IReadOnlyList<CitizenSummaryDto>>(dtos);
    }
}
