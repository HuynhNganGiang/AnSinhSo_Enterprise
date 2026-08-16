using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Households.Commands.UpdateLocation;

public sealed record UpdateHouseholdLocationCommand(
    Guid HouseholdId,
    double Latitude,
    double Longitude) : IRequest<Result>;
