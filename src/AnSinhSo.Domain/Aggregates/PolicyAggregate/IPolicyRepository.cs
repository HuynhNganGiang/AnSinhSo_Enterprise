using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Specifications;

namespace AnSinhSo.Domain.Aggregates.PolicyAggregate;

public interface IPolicyRepository
{
    Task<Policy?> GetByIdAsync(PolicyId id, CancellationToken cancellationToken = default);
    void Add(Policy policy);
    void Remove(Policy policy);

    // Specification Pattern readiness
    Task<Policy?> FirstOrDefaultAsync(ISpecification<Policy> specification, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Policy>> ListAsync(ISpecification<Policy> specification, CancellationToken cancellationToken = default);
    Task<int> CountAsync(ISpecification<Policy> specification, CancellationToken cancellationToken = default);
}
