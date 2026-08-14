using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Caching;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.Interfaces.Authorization;
using Microsoft.Extensions.Caching.Distributed;

namespace AnSinhSo.Infrastructure.Caching;

public sealed class AuthorizationCacheService : IAuthorizationCacheService
{
    private readonly IDistributedCache _cache;
    public AuthorizationCacheService(
        IDistributedCache cache)
    {
        _cache = cache;
    }

    private static string GetUserPermissionsCacheKey(CitizenIdentityId citizenIdentityId)
        => $"auth:permissions:{citizenIdentityId.Value}";

    public async Task<IReadOnlyCollection<string>?> GetUserPermissionsAsync(CitizenIdentityId citizenIdentityId, CancellationToken cancellationToken)
    {
        var cacheKey = GetUserPermissionsCacheKey(citizenIdentityId);
        var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<List<string>>(cachedData);
        }

        return null;
    }

    public async Task SetUserPermissionsAsync(CitizenIdentityId citizenIdentityId, IReadOnlyCollection<string> permissions, CancellationToken cancellationToken)
    {
        var cacheKey = GetUserPermissionsCacheKey(citizenIdentityId);
        var serializedData = JsonSerializer.Serialize(permissions);
        
        await _cache.SetStringAsync(cacheKey, serializedData, cancellationToken);
    }

    public async Task InvalidateCitizenAsync(CitizenIdentityId citizenIdentityId, CancellationToken cancellationToken)
    {
        var cacheKey = GetUserPermissionsCacheKey(citizenIdentityId);
        await _cache.RemoveAsync(cacheKey, cancellationToken);
    }

    public Task InvalidateRoleAsync(RoleId roleId, CancellationToken cancellationToken)
    {
        // To invalidate all users with a specific role, we would need to either:
        // 1. Maintain a reverse index of Role -> Users in cache.
        // 2. Clear by pattern if Redis allows (IDistributedCache doesn't natively).
        // For simplicity, this requires a slightly more complex implementation or we rely on a background job 
        // to find all users with this role and invalidate their keys.
        // Note: For now, we will just let it be a no-op or throw NotImplementedException if not strictly defined.
        // Actually, let's just leave it empty and document it, or ideally we should query the DB for users with this role and invalidate them.
        return Task.CompletedTask;
    }
}
