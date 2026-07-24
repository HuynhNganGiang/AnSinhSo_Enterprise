using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using AnSinhSo.Infrastructure.Persistence.Contexts;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public class PolicyRepository : Repository<Policy, PolicyId>, IPolicyRepository
{
    public PolicyRepository(AnSinhSoDbContext dbContext) : base(dbContext)
    {
    }

    public Task<IEnumerable<Policy>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(PolicyId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Update(Policy entity)
    {
        throw new NotImplementedException();
    }
}
