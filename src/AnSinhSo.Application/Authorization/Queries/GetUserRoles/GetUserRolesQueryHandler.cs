using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.DTOs;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetUserRoles;

public sealed class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, Result<IReadOnlyCollection<RoleDto>>>
{
    private readonly IUserRoleRepository _userRoleRepository;

    public GetUserRolesQueryHandler(IUserRoleRepository userRoleRepository)
    {
        _userRoleRepository = userRoleRepository;
    }

    public async Task<Result<IReadOnlyCollection<RoleDto>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var citizenIdentityId = new CitizenIdentityId(request.CitizenIdentityId);
        
        var roles = await _userRoleRepository.GetRolesByCitizenIdentityIdAsync(citizenIdentityId, cancellationToken);

        var dtos = roles.Select(r => new RoleDto(
            r.Id.Value,
            r.Name,
            r.Description,
            r.IsSystemRole)).ToList();

        return Result.Success<IReadOnlyCollection<RoleDto>>(dtos);
    }
}
