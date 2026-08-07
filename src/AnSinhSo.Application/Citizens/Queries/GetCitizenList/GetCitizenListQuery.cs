using System.Collections.Generic;
using AnSinhSo.Application.Citizens.DTOs;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Citizens.Queries.GetCitizenList;

public record GetCitizenListQuery() : IRequest<Result<IReadOnlyList<CitizenSummaryDto>>>;
