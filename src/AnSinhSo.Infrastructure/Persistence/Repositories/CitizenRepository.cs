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
        return _dbContext.Set<Citizen>().FirstOrDefaultAsync(x => x.CitizenNumber.Value == citizenNumber, cancellationToken);
    }

    public Task<bool> ExistsByIdAsync(CitizenId id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Citizen>().AnyAsync(x => x.Id == id, cancellationToken);
    }

    public Task<bool> ExistsByCitizenNumberAsync(string citizenNumber, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Citizen>().AnyAsync(x => x.CitizenNumber.Value == citizenNumber, cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Citizen>().AnyAsync(x => x.Email.Value == email, cancellationToken);
    }

    public Task<bool> ExistsByPhoneAsync(string phone, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Citizen>().AnyAsync(x => x.PhoneNumber.Value == phone, cancellationToken);
    }

    public async Task<(IReadOnlyList<Citizen> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? sort, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<Citizen>().AsQueryable();

        // Basic sorting could be implemented here
        query = query.OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Citizen> Items, int TotalCount)> SearchAsync(string? identityNumber, string? phone, string? keyword, int page, int pageSize, string? sort, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<Citizen>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(identityNumber))
        {
            query = query.Where(x => x.CitizenNumber.Value == identityNumber);
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            query = query.Where(x => x.PhoneNumber.Value == phone);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            // Use EF.Functions.Like for FullName or IdentityNumber or Phone
            var pattern = $"%{keyword}%";
            query = query.Where(x => 
                EF.Functions.Like(x.FullName.FirstName, pattern) ||
                EF.Functions.Like(x.FullName.LastName, pattern) ||
                EF.Functions.Like(x.CitizenNumber.Value, pattern) ||
                EF.Functions.Like(x.PhoneNumber.Value, pattern));
        }

        query = query.OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public void Add(Citizen citizen)
    {
        _dbContext.Set<Citizen>().Add(citizen);
    }

    public void Update(Citizen citizen)
    {
        _dbContext.Set<Citizen>().Update(citizen);
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
