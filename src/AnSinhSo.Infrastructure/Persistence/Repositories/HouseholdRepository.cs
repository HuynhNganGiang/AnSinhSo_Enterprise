using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Infrastructure.Persistence.Contexts;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public class HouseholdRepository : Repository<Household, HouseholdId>, IHouseholdRepository
{
    public HouseholdRepository(AnSinhSoDbContext dbContext) : base(dbContext)
    {
    }

    public Task<Household?> GetWithMembersAsync(HouseholdId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(HouseholdId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Update(Household entity)
    {
        throw new NotImplementedException();
    }
}
