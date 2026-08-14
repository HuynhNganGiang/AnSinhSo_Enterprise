using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Errors;

namespace AnSinhSo.Application.Authorization.Commands.CreateRole;

public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result<Guid>>
{
    private readonly IRoleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoleCommandHandler(
        IRoleRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsByNameAsync(request.Name, cancellationToken))
        {
            return Result.Failure<Guid>(Error.Conflict("Authorization.RoleNameExists", "A role with the same name already exists."));
        }

        var roleId = new RoleId(Guid.NewGuid());
        var createResult = Role.Create(roleId, request.Name, request.Description, request.IsSystemRole);

        if (createResult.IsFailure)
        {
            return Result.Failure<Guid>(createResult.Error);
        }

        _repository.Add(createResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(roleId.Value);
    }
}
