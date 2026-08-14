using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.DTOs;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetRoleDetail;

public sealed class GetRoleDetailQueryHandler : IRequestHandler<GetRoleDetailQuery, Result<RoleDetailDto>>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;

    public GetRoleDetailQueryHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
    }

    public async Task<Result<RoleDetailDto>> Handle(GetRoleDetailQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(new RoleId(request.RoleId), cancellationToken);
        if (role is null)
        {
            return Result.Failure<RoleDetailDto>(Error.NotFound("Authorization.RoleNotFound", "Role not found."));
        }

        var allPermissions = await _permissionRepository.GetAllAsync(cancellationToken);
        
        var rolePermissionIds = role.Permissions.Select(p => p.PermissionId).ToHashSet();
        
        var permissionDtos = allPermissions
            .Where(p => rolePermissionIds.Contains(p.Id))
            .Select(p => new PermissionDto(
                p.Id.Value,
                p.Code,
                p.Name,
                p.Description,
                p.PermissionGroupId.Value))
            .ToList();

        var detailDto = new RoleDetailDto(
            role.Id.Value,
            role.Name,
            role.Description,
            role.IsSystemRole,
            permissionDtos);

        return Result.Success(detailDto);
    }
}
