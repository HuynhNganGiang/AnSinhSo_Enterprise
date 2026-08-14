using AnSinhSo.Contracts.Citizens;
using AnSinhSo.Contracts.Common;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Citizens.Queries.GetCitizens;

public record GetCitizensQuery(int Page, int PageSize, string? Sort) : IRequest<Result<PagedResult<CitizenDto>>>;
