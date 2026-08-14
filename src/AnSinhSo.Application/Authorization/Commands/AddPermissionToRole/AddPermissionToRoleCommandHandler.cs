using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.Aggregates.PermissionAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Errors;

namespace AnSinhSo.Application.Authorization.Commands.AddPermissionToRole;

public sealed class AddPermissionToRoleCommandHandler : IRequestHandler<AddPermissionToRoleCommand, Result>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPermissionToRoleCommandHandler(
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddPermissionToRoleCommand request, CancellationToken cancellationToken)
    {
        var roleId = new RoleId(request.RoleId);
        var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken);

        if (role is null)
        {
            return Result.Failure(Error.NotFound("Authorization.RoleNotFound", "Role not found."));
        }

        var permissionId = new PermissionId(request.PermissionId);
        var permission = await _permissionRepository.GetByIdAsync(permissionId, cancellationToken);
        
        if (permission is null)
        {
            return Result.Failure(Error.NotFound("Authorization.PermissionNotFound", "Permission not found."));
        }

        var result = role.AddPermission(permissionId);
        if (result.IsFailure)
        {
            return result;
        }

        _roleRepository.Update(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
