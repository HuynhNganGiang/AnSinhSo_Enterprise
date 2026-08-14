using System.Collections.Generic;
using AnSinhSo.Application.Authorization.DTOs;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetPermissions;

public sealed record GetPermissionsQuery() : IRequest<Result<IReadOnlyCollection<PermissionDto>>>;
