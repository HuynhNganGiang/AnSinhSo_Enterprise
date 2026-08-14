using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Authorization.Commands.DeleteRole;

public sealed record DeleteRoleCommand(Guid Id) : IRequest<Result>;
