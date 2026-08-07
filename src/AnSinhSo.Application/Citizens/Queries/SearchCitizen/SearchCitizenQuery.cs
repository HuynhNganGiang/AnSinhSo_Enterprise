using System.Collections.Generic;
using AnSinhSo.Application.Citizens.DTOs;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Citizens.Queries.SearchCitizen;

public record SearchCitizenQuery(string Keyword) : IRequest<Result<IReadOnlyList<CitizenSummaryDto>>>;
