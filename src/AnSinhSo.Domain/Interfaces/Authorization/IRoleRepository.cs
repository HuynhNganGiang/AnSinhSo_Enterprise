using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.RoleAggregate;

namespace AnSinhSo.Domain.Interfaces.Authorization;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(RoleId id, CancellationToken cancellationToken = default);
    Task<System.Collections.Generic.IReadOnlyCollection<Role>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    
    void Add(Role role);
    void Update(Role role);
    void Remove(Role role);
}
