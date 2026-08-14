using System;
using System.Collections.Generic;
using AnSinhSo.Application.Authorization.DTOs;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetUserRoles;

public sealed record GetUserRolesQuery(Guid CitizenIdentityId) : IRequest<Result<IReadOnlyCollection<RoleDto>>>;
