using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Citizens.Commands.UpdateLocation;

public sealed record UpdateCitizenLocationCommand(
    Guid CitizenId,
    double Latitude,
    double Longitude) : IRequest<Result>;
