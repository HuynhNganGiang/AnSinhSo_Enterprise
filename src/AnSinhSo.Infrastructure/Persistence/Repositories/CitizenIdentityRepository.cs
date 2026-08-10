using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public sealed class CitizenIdentityRepository : ICitizenIdentityRepository
{
    private readonly AnSinhSoDbContext _dbContext;

    public CitizenIdentityRepository(AnSinhSoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<CitizenIdentity?> GetByIdAsync(CitizenIdentityId id, CancellationToken cancellationToken = default)
    {
        return _dbContext.CitizenIdentities
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<CitizenIdentity?> GetByCitizenIdAsync(CitizenId citizenId, CancellationToken cancellationToken = default)
    {
        return _dbContext.CitizenIdentities
            .FirstOrDefaultAsync(x => x.CitizenId == citizenId, cancellationToken);
    }

    public Task<CitizenIdentity?> GetByPhoneNumberAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default)
    {
        return _dbContext.CitizenIdentities
            .FirstOrDefaultAsync(x => x.PrimaryPhone != null && x.PrimaryPhone.Value == phoneNumber.Value, cancellationToken);
    }

    public Task<CitizenIdentity?> GetByLinkedProviderAsync(string providerName, string subjectId, CancellationToken cancellationToken = default)
    {
        // AD #43: No Eager loading by default, but to search inside a collection, we can use EF Core Any()
        if (!System.Enum.TryParse<ProviderType>(providerName, true, out var providerType))
        {
            return Task.FromResult<CitizenIdentity?>(null);
        }

        return _dbContext.CitizenIdentities
            .FirstOrDefaultAsync(x => x.LinkedProviders.Any(p => p.ProviderType == providerType && p.SubjectId == subjectId), cancellationToken);
    }

    public void Add(CitizenIdentity identity)
    {
        _dbContext.CitizenIdentities.Add(identity);
    }

    public void Update(CitizenIdentity identity)
    {
        _dbContext.CitizenIdentities.Update(identity);
    }
}
