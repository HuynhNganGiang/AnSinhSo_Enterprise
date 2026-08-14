using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Contracts.Citizens;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Citizens.Queries.SearchCitizenByIdentityNumber;

public class SearchCitizenByIdentityNumberQueryHandler : IRequestHandler<SearchCitizenByIdentityNumberQuery, Result<CitizenDetailDto>>
{
    private readonly ICitizenRepository _citizenRepository;

    public SearchCitizenByIdentityNumberQueryHandler(ICitizenRepository citizenRepository)
    {
        _citizenRepository = citizenRepository;
    }

    public async Task<Result<CitizenDetailDto>> Handle(SearchCitizenByIdentityNumberQuery request, CancellationToken cancellationToken)
    {
        var citizen = await _citizenRepository.GetByCitizenNumberAsync(request.IdentityNumber, cancellationToken);

        if (citizen is null)
        {
            return Result.Failure<CitizenDetailDto>(Error.NotFound("Citizen.NotFound", "Citizen not found."));
        }

        var dto = new CitizenDetailDto(
            citizen.Id.Value,
            citizen.FullName?.ToString() ?? string.Empty,
            citizen.CitizenNumber.Value,
            citizen.BirthDate,
            citizen.Gender.Name,
            citizen.PhoneNumber.Value,
            citizen.Address?.ToString() ?? string.Empty,
            citizen.Email.Value,
            citizen.Status.ToString()
        );

        return Result.Success(dto);
    }
}
