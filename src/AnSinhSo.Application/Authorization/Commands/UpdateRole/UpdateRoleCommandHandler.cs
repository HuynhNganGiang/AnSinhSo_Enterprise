using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Errors;

namespace AnSinhSo.Application.Authorization.Commands.UpdateRole;

public sealed class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Result>
{
    private readonly IRoleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoleCommandHandler(
        IRoleRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var roleId = new RoleId(request.Id);
        var role = await _repository.GetByIdAsync(roleId, cancellationToken);

        if (role is null)
        {
            return Result.Failure(Error.NotFound("Authorization.RoleNotFound", "Role not found."));
        }

        // Check if new name exists (if name is changed)
        if (role.Name != request.Name && await _repository.ExistsByNameAsync(request.Name, cancellationToken))
        {
            return Result.Failure(Error.Conflict("Authorization.RoleNameExists", "A role with the same name already exists."));
        }

        var updateResult = role.Rename(request.Name, request.Description);
        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        _repository.Update(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
