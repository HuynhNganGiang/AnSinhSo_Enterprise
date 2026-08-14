using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Authorization.Commands.AddPermissionToRole;

public sealed record AddPermissionToRoleCommand(
    Guid RoleId,
    Guid PermissionId) : IRequest<Result>;
