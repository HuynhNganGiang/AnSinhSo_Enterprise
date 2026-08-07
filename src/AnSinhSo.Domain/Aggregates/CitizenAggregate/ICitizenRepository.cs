using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Specifications;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate;

public interface ICitizenRepository
{
    Task<Citizen?> GetByIdAsync(CitizenId id, CancellationToken cancellationToken = default);
    Task<Citizen?> GetByCitizenNumberAsync(string citizenNumber, CancellationToken cancellationToken = default);
    void Add(Citizen citizen);
    void Remove(Citizen citizen);
    
    // Specification Pattern readiness
    Task<Citizen?> FirstOrDefaultAsync(ISpecification<Citizen> specification, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Citizen>> ListAsync(ISpecification<Citizen> specification, CancellationToken cancellationToken = default);
    Task<int> CountAsync(ISpecification<Citizen> specification, CancellationToken cancellationToken = default);
}
