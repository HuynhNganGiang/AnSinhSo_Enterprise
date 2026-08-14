using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.Aggregates.UserRoleAggregate;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Errors;

namespace AnSinhSo.Application.Authorization.Commands.AssignRole;

public sealed class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, Result<Guid>>
{
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ICitizenIdentityRepository _citizenIdentityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignRoleCommandHandler(
        IUserRoleRepository userRoleRepository,
        IRoleRepository roleRepository,
        ICitizenIdentityRepository citizenIdentityRepository,
        IUnitOfWork unitOfWork)
    {
        _userRoleRepository = userRoleRepository;
        _roleRepository = roleRepository;
        _citizenIdentityRepository = citizenIdentityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        var citizenIdentityId = new CitizenIdentityId(request.CitizenIdentityId);
        var citizenIdentity = await _citizenIdentityRepository.GetByIdAsync(citizenIdentityId, cancellationToken);
        if (citizenIdentity is null)
        {
            return Result.Failure<Guid>(Error.NotFound("Authorization.CitizenNotFound", "Citizen identity not found."));
        }

        var roleId = new RoleId(request.RoleId);
        var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken);
        if (role is null)
        {
            return Result.Failure<Guid>(Error.NotFound("Authorization.RoleNotFound", "Role not found."));
        }

        if (await _userRoleRepository.ExistsAsync(citizenIdentityId, roleId, cancellationToken))
        {
            return Result.Failure<Guid>(AuthorizationErrors.UserRoleAlreadyExists);
        }

        var userRoleId = new UserRoleId(Guid.NewGuid());
        var assignResult = UserRole.Assign(userRoleId, citizenIdentityId, roleId);

        if (assignResult.IsFailure)
        {
            return Result.Failure<Guid>(assignResult.Error);
        }

        _userRoleRepository.Add(assignResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(userRoleId.Value);
    }
}
