using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Caching;
using AnSinhSo.Application.Authorization.Abstractions;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Interfaces.Authorization;

namespace AnSinhSo.Infrastructure.Authorization;

public sealed class PermissionResolver : IPermissionResolver
{
    private readonly IAuthorizationCacheService _cacheService;
    private readonly IUserRoleRepository _userRoleRepository;

    public PermissionResolver(
        IAuthorizationCacheService cacheService,
        IUserRoleRepository userRoleRepository)
    {
        _cacheService = cacheService;
        _userRoleRepository = userRoleRepository;
    }

    public async Task<IReadOnlyCollection<string>> GetPermissionsAsync(CitizenIdentityId citizenIdentityId, CancellationToken cancellationToken = default)
    {
        // Cache
        var permissions = await _cacheService.GetUserPermissionsAsync(citizenIdentityId, cancellationToken);
        if (permissions != null)
        {
            return permissions;
        }

        // Repository
        permissions = await _userRoleRepository.GetPermissionsByCitizenIdentityIdAsync(citizenIdentityId, cancellationToken);

        // Set Cache
        await _cacheService.SetUserPermissionsAsync(citizenIdentityId, permissions, cancellationToken);

        return permissions;
    }
}
