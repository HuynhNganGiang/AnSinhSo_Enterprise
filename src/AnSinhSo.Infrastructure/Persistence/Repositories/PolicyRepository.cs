using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using AnSinhSo.Domain.Specifications;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public sealed class PolicyRepository : IPolicyRepository
{
    private readonly AnSinhSoDbContext _dbContext;

    public PolicyRepository(AnSinhSoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Policy?> GetByIdAsync(PolicyId id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Policy>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Add(Policy policy)
    {
        _dbContext.Set<Policy>().Add(policy);
    }

    public void Remove(Policy policy)
    {
        _dbContext.Set<Policy>().Remove(policy);
    }

    public Task<Policy?> FirstOrDefaultAsync(ISpecification<Policy> specification, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Policy>().Where(specification.ToExpression()).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Policy>> ListAsync(ISpecification<Policy> specification, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Policy>().Where(specification.ToExpression()).ToListAsync(cancellationToken);
    }

    public Task<int> CountAsync(ISpecification<Policy> specification, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Policy>().Where(specification.ToExpression()).CountAsync(cancellationToken);
    }
}
