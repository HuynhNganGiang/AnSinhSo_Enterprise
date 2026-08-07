using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Specifications;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate;

public interface IHouseholdRepository
{
    Task<Household?> GetByIdAsync(HouseholdId id, CancellationToken cancellationToken = default);
    void Add(Household household);
    void Remove(Household household);

    // Specification Pattern readiness
    Task<Household?> FirstOrDefaultAsync(ISpecification<Household> specification, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Household>> ListAsync(ISpecification<Household> specification, CancellationToken cancellationToken = default);
    Task<int> CountAsync(ISpecification<Household> specification, CancellationToken cancellationToken = default);
}
