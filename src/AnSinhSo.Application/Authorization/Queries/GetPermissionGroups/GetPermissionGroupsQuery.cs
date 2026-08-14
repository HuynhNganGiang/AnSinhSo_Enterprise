using System.Collections.Generic;
using AnSinhSo.Application.Authorization.DTOs;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetPermissionGroups;

public sealed record GetPermissionGroupsQuery() : IRequest<Result<IReadOnlyCollection<PermissionGroupWithPermissionsDto>>>;
