using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Authorization.Commands.RevokeRole;

public sealed record RevokeRoleCommand(
    Guid CitizenIdentityId,
    Guid RoleId) : IRequest<Result>;
