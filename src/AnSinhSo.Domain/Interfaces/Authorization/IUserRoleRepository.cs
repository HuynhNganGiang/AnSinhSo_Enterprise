using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.PermissionAggregate;
using AnSinhSo.Domain.Aggregates.UserRoleAggregate;

namespace AnSinhSo.Domain.Interfaces.Authorization;

public interface IUserRoleRepository
{
    Task<UserRole?> GetByIdAsync(UserRoleId id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all permissions for a given citizen by evaluating the union of permissions from all their assigned roles.
    /// </summary>
    Task<IReadOnlyCollection<string>> GetPermissionsByCitizenIdentityIdAsync(CitizenIdentityId id, CancellationToken cancellationToken = default);
    
    void Add(UserRole userRole);
    void Remove(UserRole userRole);
}
