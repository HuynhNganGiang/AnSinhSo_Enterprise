using AnSinhSo.Contracts.Citizens;
using AnSinhSo.Contracts.Common;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Citizens.Queries.SearchCitizen;

public record SearchCitizenQuery(string? IdentityNumber, string? Phone, string? Keyword, int Page, int PageSize, string? Sort) : IRequest<Result<PagedResult<CitizenDto>>>;
