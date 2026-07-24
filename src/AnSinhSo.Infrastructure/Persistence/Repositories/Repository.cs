using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.Specifications;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public class Repository<TAggregate, TId> : IRepository<TAggregate, TId>
    where TAggregate : AggregateRoot<TId>
{
    protected readonly AnSinhSoDbContext _dbContext;
    protected readonly DbSet<TAggregate> _dbSet;

    public Repository(AnSinhSoDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<TAggregate>();
    }

    public virtual async Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        if (id == null) return null;
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual async Task AddAsync(TAggregate entity, CancellationToken cancellationToken = default)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public virtual void Remove(TAggregate entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _dbSet.Remove(entity);
    }

    public virtual Task<TAggregate?> FirstOrDefaultAsync(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<IReadOnlyList<TAggregate>> ListAsync(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<int> CountAsync(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
