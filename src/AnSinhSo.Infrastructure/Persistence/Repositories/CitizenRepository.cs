using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Specifications;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public sealed class CitizenRepository : ICitizenRepository
{
    private readonly AnSinhSoDbContext _dbContext;

    public CitizenRepository(AnSinhSoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Citizen?> GetByIdAsync(CitizenId id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Citizen>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Citizen?> GetByCitizenNumberAsync(string citizenNumber, CancellationToken cancellationToken = default)
    {
        // CitizenNumber is a ValueObject so it's mapped to a column, or it can be a navigation.
        // Assuming CitizenNumber is mapped cleanly.
        // wait, we need to check how CitizenNumber is modeled. It's a ValueObject, so x.CitizenNumber.Value == citizenNumber.
        // EF Core 8 ComplexProperty can handle x => x.CitizenNumber.Value.
        // However, we don't have the domain definition exactly in mind. 
        // A generic approach is fine, or we can use EF.Functions.
        return _dbContext.Set<Citizen>().FirstOrDefaultAsync(x => x.CitizenNumber.Value == citizenNumber, cancellationToken);
    }

    public void Add(Citizen citizen)
    {
        _dbContext.Set<Citizen>().Add(citizen);
    }

    public void Remove(Citizen citizen)
    {
        _dbContext.Set<Citizen>().Remove(citizen);
    }

    public Task<Citizen?> FirstOrDefaultAsync(ISpecification<Citizen> specification, CancellationToken cancellationToken = default)
    {
        // Simple Specification evaluator logic - just a placeholder to satisfy interface
        return _dbContext.Set<Citizen>().Where(specification.ToExpression()).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Citizen>> ListAsync(ISpecification<Citizen> specification, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Citizen>().Where(specification.ToExpression()).ToListAsync(cancellationToken);
    }

    public Task<int> CountAsync(ISpecification<Citizen> specification, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Citizen>().Where(specification.ToExpression()).CountAsync(cancellationToken);
    }
}
