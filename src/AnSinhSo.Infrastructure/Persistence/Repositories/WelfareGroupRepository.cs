using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;
using AnSinhSo.Domain.Specifications;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public sealed class WelfareGroupRepository : IWelfareGroupRepository
{
    private readonly AnSinhSoDbContext _dbContext;

    public WelfareGroupRepository(AnSinhSoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<WelfareGroup?> GetByIdAsync(WelfareGroupId id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<WelfareGroup>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Add(WelfareGroup welfareGroup)
    {
        _dbContext.Set<WelfareGroup>().Add(welfareGroup);
    }

    public void Remove(WelfareGroup welfareGroup)
    {
        _dbContext.Set<WelfareGroup>().Remove(welfareGroup);
    }

    public Task<WelfareGroup?> FirstOrDefaultAsync(ISpecification<WelfareGroup> specification, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<WelfareGroup>().Where(specification.ToExpression()).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<WelfareGroup>> ListAsync(ISpecification<WelfareGroup> specification, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<WelfareGroup>().Where(specification.ToExpression()).ToListAsync(cancellationToken);
    }

    public Task<int> CountAsync(ISpecification<WelfareGroup> specification, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<WelfareGroup>().Where(specification.ToExpression()).CountAsync(cancellationToken);
    }
}
