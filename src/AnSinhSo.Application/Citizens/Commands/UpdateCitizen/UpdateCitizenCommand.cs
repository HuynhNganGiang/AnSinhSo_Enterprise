using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Citizens.Commands.UpdateCitizen;

public sealed record UpdateCitizenCommand(
    Guid CitizenId,
    string PhoneNumber,
    string Address) : IRequest<Result>;
