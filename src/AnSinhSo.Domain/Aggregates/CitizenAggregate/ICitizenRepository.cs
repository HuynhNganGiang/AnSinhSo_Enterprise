using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Specifications;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate;

public interface ICitizenRepository
{
    Task<Citizen?> GetByIdAsync(CitizenId id, CancellationToken cancellationToken = default);
    Task<Citizen?> GetByCitizenNumberAsync(string citizenNumber, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(CitizenId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCitizenNumberAsync(string citizenNumber, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByPhoneAsync(string phone, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Citizen> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? sort, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Citizen> Items, int TotalCount)> SearchAsync(string? identityNumber, string? phone, string? keyword, int page, int pageSize, string? sort, CancellationToken cancellationToken = default);

    void Add(Citizen citizen);
    void Update(Citizen citizen);
    void Remove(Citizen citizen);
    
    // Specification Pattern readiness
    Task<Citizen?> FirstOrDefaultAsync(ISpecification<Citizen> specification, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Citizen>> ListAsync(ISpecification<Citizen> specification, CancellationToken cancellationToken = default);
    Task<int> CountAsync(ISpecification<Citizen> specification, CancellationToken cancellationToken = default);
}
