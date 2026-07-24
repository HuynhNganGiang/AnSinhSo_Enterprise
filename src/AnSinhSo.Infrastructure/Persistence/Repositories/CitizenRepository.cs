using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.ValueObjects;
using AnSinhSo.Infrastructure.Persistence.Contexts;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public class CitizenRepository : Repository<Citizen, CitizenId>, ICitizenRepository
{
    public CitizenRepository(AnSinhSoDbContext dbContext) : base(dbContext)
    {
    }

    public Task<Citizen?> GetByCitizenNumberAsync(CitizenNumber citizenNumber, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsCitizenNumberAsync(CitizenNumber citizenNumber, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(CitizenId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Update(Citizen entity)
    {
        throw new NotImplementedException();
    }
}
