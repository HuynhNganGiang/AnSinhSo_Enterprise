using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.RelationshipTypeAggregate;
using AnSinhSo.Domain.Specifications;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public sealed class RelationshipTypeRepository : IRelationshipTypeRepository
{
    private readonly AnSinhSoDbContext _dbContext;

    public RelationshipTypeRepository(AnSinhSoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<RelationshipType?> GetByIdAsync(RelationshipTypeId id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<RelationshipType>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<RelationshipType?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<RelationshipType>().FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }

    public Task<bool> ExistsByIdAsync(RelationshipTypeId id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<RelationshipType>().AnyAsync(x => x.Id == id, cancellationToken);
    }

    public Task<bool> ExistsAsync(RelationshipTypeId id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<RelationshipType>().AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<RelationshipType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<RelationshipType>()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RelationshipType>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<RelationshipType>()
            .Where(x => x.IsActive)
            .ToListAsync(cancellationToken);
    }

    public void Add(RelationshipType relationshipType)
    {
        _dbContext.Set<RelationshipType>().Add(relationshipType);
    }

    public void Remove(RelationshipType relationshipType)
    {
        _dbContext.Set<RelationshipType>().Remove(relationshipType);
    }

    public Task<RelationshipType?> FirstOrDefaultAsync(ISpecification<RelationshipType> specification, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<RelationshipType>().Where(specification.ToExpression()).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RelationshipType>> ListAsync(ISpecification<RelationshipType> specification, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<RelationshipType>().Where(specification.ToExpression()).ToListAsync(cancellationToken);
    }

    public Task<int> CountAsync(ISpecification<RelationshipType> specification, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<RelationshipType>().Where(specification.ToExpression()).CountAsync(cancellationToken);
    }
}
