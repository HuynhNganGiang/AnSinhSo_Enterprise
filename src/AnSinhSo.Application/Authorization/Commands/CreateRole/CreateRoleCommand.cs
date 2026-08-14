using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Authorization.Commands.CreateRole;

public sealed record CreateRoleCommand(
    string Name,
    string Description,
    bool IsSystemRole) : IRequest<Result<Guid>>;
