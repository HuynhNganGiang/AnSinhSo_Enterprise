using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Authorization.Commands.DeletePermissionGroup;

public sealed record DeletePermissionGroupCommand(Guid Id) : IRequest<Result>;
