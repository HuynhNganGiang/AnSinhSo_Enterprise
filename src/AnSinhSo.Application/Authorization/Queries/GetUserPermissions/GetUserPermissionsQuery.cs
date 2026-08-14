using System;
using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetUserPermissions;

public sealed record GetUserPermissionsQuery(Guid CitizenIdentityId) : IRequest<Result<IReadOnlyCollection<string>>>;
