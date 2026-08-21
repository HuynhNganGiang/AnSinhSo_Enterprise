using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.ZaloUserAggregate;

namespace AnSinhSo.Domain.Interfaces.Repositories;

public interface IZaloUserRepository
{
    Task<ZaloUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ZaloUser?> GetByZaloIdAsync(string zaloId, CancellationToken cancellationToken = default);
    Task<ZaloUser?> GetByCitizenIdAsync(Guid citizenId, CancellationToken cancellationToken = default);
    Task<ZaloUser?> GetByCitizenIdentityIdAsync(Guid citizenIdentityId, CancellationToken cancellationToken = default);
    void Add(ZaloUser zaloUser);
    void Update(ZaloUser zaloUser);
}
