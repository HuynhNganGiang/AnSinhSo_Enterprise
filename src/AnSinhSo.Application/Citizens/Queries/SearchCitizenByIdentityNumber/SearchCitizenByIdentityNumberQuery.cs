using AnSinhSo.Contracts.Citizens;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Citizens.Queries.SearchCitizenByIdentityNumber;

public record SearchCitizenByIdentityNumberQuery(string IdentityNumber) : IRequest<Result<CitizenDetailDto>>;
