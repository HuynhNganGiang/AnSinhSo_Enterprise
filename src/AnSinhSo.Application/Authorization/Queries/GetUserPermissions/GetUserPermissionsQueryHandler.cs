using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Caching;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetUserPermissions;

public sealed class GetUserPermissionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, Result<IReadOnlyCollection<string>>>
{
    private readonly IAuthorizationCacheService _cacheService;
    private readonly IUserRoleRepository _userRoleRepository;

    public GetUserPermissionsQueryHandler(
        IAuthorizationCacheService cacheService,
        IUserRoleRepository userRoleRepository)
    {
        _cacheService = cacheService;
        _userRoleRepository = userRoleRepository;
    }

    public async Task<Result<IReadOnlyCollection<string>>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
    {
        var citizenIdentityId = new CitizenIdentityId(request.CitizenIdentityId);
        
        // 1. Try get from cache
        var cachedPermissions = await _cacheService.GetUserPermissionsAsync(citizenIdentityId, cancellationToken);
        if (cachedPermissions != null)
        {
            return Result.Success(cachedPermissions);
        }

        // 2. Cache miss -> Get from database
        var dbPermissions = await _userRoleRepository.GetPermissionsByCitizenIdentityIdAsync(citizenIdentityId, cancellationToken);

        // 3. Save to cache
        await _cacheService.SetUserPermissionsAsync(citizenIdentityId, dbPermissions, cancellationToken);

        // 4. Return
        return Result.Success(dbPermissions);
    }
}
