using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Errors;

namespace AnSinhSo.Application.Authorization.Commands.DeleteRole;

public sealed class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Result>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoleCommandHandler(
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var roleId = new RoleId(request.Id);
        var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken);

        if (role is null)
        {
            return Result.Failure(Error.NotFound("Authorization.RoleNotFound", "Role not found."));
        }

        if (role.IsSystemRole)
        {
            return Result.Failure(AuthorizationErrors.CannotModifySystemRole);
        }

        if (await _userRoleRepository.ExistsByRoleIdAsync(roleId, cancellationToken))
        {
            return Result.Failure(AuthorizationErrors.RoleInUse);
        }

        _roleRepository.Remove(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
