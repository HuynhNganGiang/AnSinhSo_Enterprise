using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.WelfarePrograms.DTOs;
using AnSinhSo.Domain.Aggregates.WelfareProgramAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.WelfarePrograms.Queries.GetWelfarePrograms;

public record GetWelfareProgramsQuery() : IRequest<Result<IReadOnlyList<WelfareProgramDto>>>;

public class GetWelfareProgramsQueryHandler : IRequestHandler<GetWelfareProgramsQuery, Result<IReadOnlyList<WelfareProgramDto>>>
{
    private readonly IWelfareProgramRepository _repository;

    public GetWelfareProgramsQueryHandler(IWelfareProgramRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IReadOnlyList<WelfareProgramDto>>> Handle(GetWelfareProgramsQuery request, CancellationToken cancellationToken)
    {
        var programs = await _repository.GetAllActiveAsync(cancellationToken);
        
        var dtos = programs.Select(p => new WelfareProgramDto(
            p.Id.Value,
            p.Code,
            p.Name,
            p.Description,
            p.IsActive)).ToList();

        return Result.Success<IReadOnlyList<WelfareProgramDto>>(dtos);
    }
}
