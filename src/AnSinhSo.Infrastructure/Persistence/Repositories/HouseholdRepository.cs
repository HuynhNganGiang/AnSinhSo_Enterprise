using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate.Enumerations;
using AnSinhSo.Domain.ValueObjects;
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
        return _dbContext.Set<Household>()
            .Include(h => h.Members)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Household?> GetByCodeAsync(HouseholdCode code, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Household>()
            .Include(h => h.Members)
            .FirstOrDefaultAsync(x => x.HouseholdCode == code, cancellationToken);
    }

    public Task<Household?> GetByCitizenIdAsync(CitizenId citizenId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Household>()
            .Include(h => h.Members)
            .FirstOrDefaultAsync(x => x.Members.Any(m => m.CitizenId == citizenId), cancellationToken);
    }

    public Task<bool> ExistsByIdAsync(HouseholdId id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Household>().AnyAsync(x => x.Id == id, cancellationToken);
    }

    public Task<bool> ExistsByCodeAsync(HouseholdCode code, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Household>().AnyAsync(x => x.HouseholdCode == code, cancellationToken);
    }

    public Task<bool> IsCitizenInAnyHouseholdAsync(CitizenId citizenId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Household>()
            .AnyAsync(x => x.Members.Any(m => m.CitizenId == citizenId), cancellationToken);
    }

    public async Task<(IReadOnlyList<Household> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? sort, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<Household>().AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(sort))
        {
            // Pending: implement sort
        }

        var items = await query
            .Include(h => h.Members)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<Household> Items, int TotalCount)> SearchAsync(string? keyword, HouseholdStatus? status, int page, int pageSize, string? sort, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<Household>().AsQueryable();

        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var lowerKeyword = keyword.ToLower();
            query = query.Where(x => x.HouseholdCode.Value.ToLower().Contains(lowerKeyword));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(sort))
        {
            // Pending: implement sort
        }

        var items = await query
            .Include(h => h.Members)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public void Add(Household household)
    {
        _dbContext.Set<Household>().Add(household);
    }

    public void Update(Household household)
    {
        _dbContext.Set<Household>().Update(household);
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
