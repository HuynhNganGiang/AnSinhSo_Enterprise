using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.DTOs;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetPermissions;

public sealed class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, Result<IReadOnlyCollection<PermissionDto>>>
{
    private readonly IPermissionRepository _repository;

    public GetPermissionsQueryHandler(IPermissionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IReadOnlyCollection<PermissionDto>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _repository.GetAllAsync(cancellationToken);

        var dtos = permissions.Select(p => new PermissionDto(
            p.Id.Value,
            p.Code,
            p.Name,
            p.Description,
            p.PermissionGroupId.Value)).ToList();

        return Result.Success<IReadOnlyCollection<PermissionDto>>(dtos);
    }
}
