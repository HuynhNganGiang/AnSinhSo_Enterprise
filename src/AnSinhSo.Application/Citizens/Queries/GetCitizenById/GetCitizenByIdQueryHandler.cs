using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Citizens.DTOs;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using AutoMapper;
using MediatR;

namespace AnSinhSo.Application.Citizens.Queries.GetCitizenById;

public class GetCitizenByIdQueryHandler : IRequestHandler<GetCitizenByIdQuery, Result<CitizenDto>>
{
    private readonly ICitizenRepository _citizenRepository;
    private readonly IMapper _mapper;

    public GetCitizenByIdQueryHandler(ICitizenRepository citizenRepository, IMapper mapper)
    {
        _citizenRepository = citizenRepository;
        _mapper = mapper;
    }

    public async Task<Result<CitizenDto>> Handle(GetCitizenByIdQuery request, CancellationToken cancellationToken)
    {
        var citizen = await _citizenRepository.GetByIdAsync(new CitizenId(request.CitizenId), cancellationToken);

        if (citizen is null)
        {
            return Result.Failure<CitizenDto>(Error.NotFound("Citizen.NotFound", "Citizen not found."));
        }

        var dto = _mapper.Map<CitizenDto>(citizen);

        return Result.Success(dto);
    }
}
