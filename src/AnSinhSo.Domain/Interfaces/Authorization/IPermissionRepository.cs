using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.PermissionAggregate;

namespace AnSinhSo.Domain.Interfaces.Authorization;

public interface IPermissionRepository
{
    Task<Permission?> GetByIdAsync(PermissionId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
    
    void Add(Permission permission);
    void Update(Permission permission);
    void Remove(Permission permission);
}
