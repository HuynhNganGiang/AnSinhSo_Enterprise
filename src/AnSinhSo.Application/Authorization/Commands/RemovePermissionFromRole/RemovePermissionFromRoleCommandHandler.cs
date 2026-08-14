using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.Aggregates.PermissionAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Errors;

namespace AnSinhSo.Application.Authorization.Commands.RemovePermissionFromRole;

public sealed class RemovePermissionFromRoleCommandHandler : IRequestHandler<RemovePermissionFromRoleCommand, Result>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemovePermissionFromRoleCommandHandler(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemovePermissionFromRoleCommand request, CancellationToken cancellationToken)
    {
        var roleId = new RoleId(request.RoleId);
        var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken);

        if (role is null)
        {
            return Result.Failure(Error.NotFound("Authorization.RoleNotFound", "Role not found."));
        }

        var permissionId = new PermissionId(request.PermissionId);
        
        var result = role.RemovePermission(permissionId);
        if (result.IsFailure)
        {
            return result;
        }

        _roleRepository.Update(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
