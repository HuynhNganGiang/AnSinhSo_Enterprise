using System;
using AnSinhSo.Contracts.Citizens;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Citizens.Queries.GetCitizenById;

public record GetCitizenByIdQuery(Guid CitizenId) : IRequest<Result<CitizenDetailDto>>;
