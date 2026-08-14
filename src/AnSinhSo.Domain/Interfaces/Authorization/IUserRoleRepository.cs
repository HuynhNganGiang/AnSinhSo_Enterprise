using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.PermissionAggregate;
using AnSinhSo.Domain.Aggregates.UserRoleAggregate;
using AnSinhSo.Domain.Aggregates.RoleAggregate;

namespace AnSinhSo.Domain.Interfaces.Authorization;

public interface IUserRoleRepository
{
    Task<UserRole?> GetByIdAsync(UserRoleId id, CancellationToken cancellationToken = default);
    Task<UserRole?> GetByCitizenAndRoleAsync(CitizenIdentityId citizenIdentityId, RoleId roleId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByRoleIdAsync(RoleId roleId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(CitizenIdentityId citizenIdentityId, RoleId roleId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Role>> GetRolesByCitizenIdentityIdAsync(CitizenIdentityId citizenIdentityId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all permissions for a given citizen by evaluating the union of permissions from all their assigned roles.
    /// </summary>
    Task<IReadOnlyCollection<string>> GetPermissionsByCitizenIdentityIdAsync(CitizenIdentityId id, CancellationToken cancellationToken = default);
    
    void Add(UserRole userRole);
    void Remove(UserRole userRole);
}
