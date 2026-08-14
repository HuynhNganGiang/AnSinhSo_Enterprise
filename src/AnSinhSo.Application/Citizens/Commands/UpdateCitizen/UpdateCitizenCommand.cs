using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Citizens.Commands.UpdateCitizen;

public sealed record UpdateCitizenCommand(
    Guid CitizenId,
    string FullName,
    DateTime BirthDate,
    int Gender,
    string PhoneNumber,
    string Address,
    string Email) : IRequest<Result>;
