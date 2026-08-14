using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.PermissionGroupAggregate;

namespace AnSinhSo.Domain.Interfaces.Authorization;

public interface IPermissionGroupRepository
{
    Task<PermissionGroup?> GetByIdAsync(PermissionGroupId id, CancellationToken cancellationToken = default);
    Task<System.Collections.Generic.IReadOnlyCollection<PermissionGroup>> GetAllAsync(CancellationToken cancellationToken = default);
    
    void Add(PermissionGroup permissionGroup);
    void Update(PermissionGroup permissionGroup);
    void Remove(PermissionGroup permissionGroup);
}
