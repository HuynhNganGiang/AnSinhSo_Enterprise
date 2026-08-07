using System;
using AnSinhSo.Application.Citizens.DTOs;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Citizens.Queries.GetCitizenById;

public record GetCitizenByIdQuery(Guid CitizenId) : IRequest<Result<CitizenDto>>;
