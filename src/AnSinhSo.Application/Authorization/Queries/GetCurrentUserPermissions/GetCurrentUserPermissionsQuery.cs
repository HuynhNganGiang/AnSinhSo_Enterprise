using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetCurrentUserPermissions;

public sealed record GetCurrentUserPermissionsQuery() : IRequest<Result<List<string>>>;
