using System.Collections.Generic;
using AnSinhSo.Contracts.Authorization;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetCurrentUserRoles;

public sealed record GetCurrentUserRolesQuery() : IRequest<Result<List<RoleDto>>>;
