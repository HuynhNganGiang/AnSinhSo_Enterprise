using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.CitizenIdentities.Commands.RegisterCitizenIdentity;

public sealed record RegisterCitizenIdentityCommand(
    Guid CitizenId,
    string PhoneNumber
) : IRequest<Result<Guid>>;
