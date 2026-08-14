using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.Aggregates.PermissionGroupAggregate;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Authorization.Commands.CreatePermissionGroup;

public sealed class CreatePermissionGroupCommandHandler : IRequestHandler<CreatePermissionGroupCommand, Result<Guid>>
{
    private readonly IPermissionGroupRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePermissionGroupCommandHandler(
        IPermissionGroupRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreatePermissionGroupCommand request, CancellationToken cancellationToken)
    {
        var groupId = new PermissionGroupId(Guid.NewGuid());
        var createResult = PermissionGroup.Create(groupId, request.Code, request.Name, request.Description);

        if (createResult.IsFailure)
        {
            return Result.Failure<Guid>(createResult.Error);
        }

        _repository.Add(createResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(groupId.Value);
    }
}
