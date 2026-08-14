using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Errors;

namespace AnSinhSo.Application.Authorization.Commands.RevokeRole;

public sealed class RevokeRoleCommandHandler : IRequestHandler<RevokeRoleCommand, Result>
{
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RevokeRoleCommandHandler(
        IUserRoleRepository userRoleRepository,
        IUnitOfWork unitOfWork)
    {
        _userRoleRepository = userRoleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RevokeRoleCommand request, CancellationToken cancellationToken)
    {
        var citizenIdentityId = new CitizenIdentityId(request.CitizenIdentityId);
        var roleId = new RoleId(request.RoleId);
        
        var userRole = await _userRoleRepository.GetByCitizenAndRoleAsync(citizenIdentityId, roleId, cancellationToken);
        
        if (userRole is null)
        {
            return Result.Failure(AuthorizationErrors.UserRoleNotFound);
        }

        var revokeResult = userRole.Revoke();
        if (revokeResult.IsFailure)
        {
            return revokeResult;
        }

        _userRoleRepository.Remove(userRole);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
