using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Authorization.Commands.CreatePermissionGroup;

public sealed record CreatePermissionGroupCommand(
    string Code,
    string Name,
    string Description) : IRequest<Result<Guid>>;
