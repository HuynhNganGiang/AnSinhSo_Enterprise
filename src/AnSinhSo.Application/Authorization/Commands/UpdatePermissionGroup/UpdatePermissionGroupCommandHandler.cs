using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.Aggregates.PermissionGroupAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Errors;

namespace AnSinhSo.Application.Authorization.Commands.UpdatePermissionGroup;

public sealed class UpdatePermissionGroupCommandHandler : IRequestHandler<UpdatePermissionGroupCommand, Result>
{
    private readonly IPermissionGroupRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePermissionGroupCommandHandler(
        IPermissionGroupRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdatePermissionGroupCommand request, CancellationToken cancellationToken)
    {
        var groupId = new PermissionGroupId(request.Id);
        var group = await _repository.GetByIdAsync(groupId, cancellationToken);

        if (group is null)
        {
            return Result.Failure(Error.NotFound("Authorization.PermissionGroupNotFound", "Permission group not found."));
        }

        var updateResult = group.Rename(request.Name, request.Description);
        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        _repository.Update(group);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
