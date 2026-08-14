using System.Collections.Generic;
using AnSinhSo.Application.Authorization.DTOs;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetRoles;

public sealed record GetRolesQuery() : IRequest<Result<IReadOnlyCollection<RoleDto>>>;
