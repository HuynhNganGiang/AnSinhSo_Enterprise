using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Authorization.Commands.UpdateRole;

public sealed record UpdateRoleCommand(
    Guid Id,
    string Name,
    string Description) : IRequest<Result>;
