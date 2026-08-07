using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Specifications;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public sealed class HouseholdRepository : IHouseholdRepository
{
    private readonly AnSinhSoDbContext _dbContext;

    public HouseholdRepository(AnSinhSoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Household?> GetByIdAsync(HouseholdId id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Household>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Add(Household household)
    {
        _dbContext.Set<Household>().Add(household);
    }

    public void Remove(Household household)
    {
        _dbContext.Set<Household>().Remove(household);
    }

    public Task<Household?> FirstOrDefaultAsync(ISpecification<Household> specification, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Household>().Where(specification.ToExpression()).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Household>> ListAsync(ISpecification<Household> specification, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Household>().Where(specification.ToExpression()).ToListAsync(cancellationToken);
    }

    public Task<int> CountAsync(ISpecification<Household> specification, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Household>().Where(specification.ToExpression()).CountAsync(cancellationToken);
    }
}
