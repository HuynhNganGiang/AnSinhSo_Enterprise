using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.Aggregates.PermissionAggregate;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;
using System.Linq;

namespace AnSinhSo.Application.Authorization.Commands.UpdateRolePermissions;

internal sealed class UpdateRolePermissionsCommandHandler : IRequestHandler<UpdateRolePermissionsCommand, Result>
{
    private readonly IRoleRepository _roleRepository;

    public UpdateRolePermissionsCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(new RoleId(request.RoleId), cancellationToken);

        if (role == null)
        {
            return Result.Failure(AuthorizationErrors.RoleNotFound);
        }

        var newPermissions = request.PermissionIds.Select(id => new PermissionId(id)).ToList();
        
        var result = role.UpdatePermissions(newPermissions);
        
        if (result.IsFailure)
        {
            return result;
        }

        _roleRepository.Update(role);
        
        return Result.Success();
    }
}
