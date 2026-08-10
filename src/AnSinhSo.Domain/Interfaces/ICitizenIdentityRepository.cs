using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;

public interface ICitizenIdentityRepository
{
    Task<CitizenIdentity?> GetByIdAsync(CitizenIdentityId id, CancellationToken cancellationToken = default);
    Task<CitizenIdentity?> GetByCitizenIdAsync(CitizenId citizenId, CancellationToken cancellationToken = default);
    Task<CitizenIdentity?> GetByPhoneNumberAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default);
    Task<CitizenIdentity?> GetByLinkedProviderAsync(string providerName, string subjectId, CancellationToken cancellationToken = default);
    
    void Add(CitizenIdentity identity);
    void Update(CitizenIdentity identity);
}
