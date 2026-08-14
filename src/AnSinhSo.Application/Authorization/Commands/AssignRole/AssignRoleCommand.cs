using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Authorization.Commands.AssignRole;

public sealed record AssignRoleCommand(
    Guid CitizenIdentityId,
    Guid RoleId) : IRequest<Result<Guid>>;
