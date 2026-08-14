using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.DTOs;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetPermissionGroups;

public sealed class GetPermissionGroupsQueryHandler : IRequestHandler<GetPermissionGroupsQuery, Result<IReadOnlyCollection<PermissionGroupWithPermissionsDto>>>
{
    private readonly IPermissionGroupRepository _groupRepository;
    private readonly IPermissionRepository _permissionRepository;

    public GetPermissionGroupsQueryHandler(
        IPermissionGroupRepository groupRepository,
        IPermissionRepository permissionRepository)
    {
        _groupRepository = groupRepository;
        _permissionRepository = permissionRepository;
    }

    public async Task<Result<IReadOnlyCollection<PermissionGroupWithPermissionsDto>>> Handle(GetPermissionGroupsQuery request, CancellationToken cancellationToken)
    {
        var groups = await _groupRepository.GetAllAsync(cancellationToken);
        var permissions = await _permissionRepository.GetAllAsync(cancellationToken);

        var dtos = groups.Select(g => new PermissionGroupWithPermissionsDto(
            g.Id.Value,
            g.Code,
            g.Name,
            g.Description,
            permissions.Where(p => p.PermissionGroupId == g.Id)
                       .Select(p => new PermissionDto(p.Id.Value, p.Code, p.Name, p.Description, p.PermissionGroupId.Value))
                       .ToList()
        )).ToList();

        return Result.Success<IReadOnlyCollection<PermissionGroupWithPermissionsDto>>(dtos);
    }
}
