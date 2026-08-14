using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.RoleAggregate;

namespace AnSinhSo.Application.Abstractions.Caching;

public interface IAuthorizationCacheService
{
    Task<IReadOnlyCollection<string>?> GetUserPermissionsAsync(CitizenIdentityId citizenIdentityId, CancellationToken cancellationToken);
    Task SetUserPermissionsAsync(CitizenIdentityId citizenIdentityId, IReadOnlyCollection<string> permissions, CancellationToken cancellationToken);
    
    Task InvalidateCitizenAsync(CitizenIdentityId citizenIdentityId, CancellationToken cancellationToken);
    Task InvalidateRoleAsync(RoleId roleId, CancellationToken cancellationToken);
}
