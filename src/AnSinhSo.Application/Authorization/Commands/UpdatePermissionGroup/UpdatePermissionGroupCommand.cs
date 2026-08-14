using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Authorization.Commands.UpdatePermissionGroup;

public sealed record UpdatePermissionGroupCommand(
    Guid Id,
    string Name,
    string Description) : IRequest<Result>;
