using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Authorization.Commands.RemovePermissionFromRole;

public sealed record RemovePermissionFromRoleCommand(
    Guid RoleId,
    Guid PermissionId) : IRequest<Result>;
