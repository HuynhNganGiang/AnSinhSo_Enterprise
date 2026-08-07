using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Specifications;

namespace AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;

public interface IWelfareGroupRepository
{
    Task<WelfareGroup?> GetByIdAsync(WelfareGroupId id, CancellationToken cancellationToken = default);
    void Add(WelfareGroup welfareGroup);
    void Remove(WelfareGroup welfareGroup);

    // Specification Pattern readiness
    Task<WelfareGroup?> FirstOrDefaultAsync(ISpecification<WelfareGroup> specification, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WelfareGroup>> ListAsync(ISpecification<WelfareGroup> specification, CancellationToken cancellationToken = default);
    Task<int> CountAsync(ISpecification<WelfareGroup> specification, CancellationToken cancellationToken = default);
}
