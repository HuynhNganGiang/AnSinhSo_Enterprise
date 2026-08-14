using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.Aggregates.PermissionGroupAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Errors;

namespace AnSinhSo.Application.Authorization.Commands.DeletePermissionGroup;

public sealed class DeletePermissionGroupCommandHandler : IRequestHandler<DeletePermissionGroupCommand, Result>
{
    private readonly IPermissionGroupRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePermissionGroupCommandHandler(
        IPermissionGroupRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeletePermissionGroupCommand request, CancellationToken cancellationToken)
    {
        var groupId = new PermissionGroupId(request.Id);
        var group = await _repository.GetByIdAsync(groupId, cancellationToken);

        if (group is null)
        {
            return Result.Failure(Error.NotFound("Authorization.PermissionGroupNotFound", "Permission group not found."));
        }

        // Ideally, check if any permissions are using this group before deletion.
        // Assuming we can just delete it for now as per simple CRUD.
        _repository.Remove(group);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
