using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate.ValueObjects;

namespace AnSinhSo.Domain.Interfaces;

public interface IOtpVerificationRepository
{
    Task<OtpVerification?> GetByIdAsync(OtpVerificationId id, CancellationToken cancellationToken = default);
    Task<OtpVerification?> GetPendingByCitizenIdentityIdAsync(CitizenIdentityId citizenIdentityId, CancellationToken cancellationToken = default);
    Task<OtpVerification?> GetByRequestIdAsync(System.Guid requestId, CancellationToken cancellationToken = default);
    
    void Add(OtpVerification otpVerification);
    void Update(OtpVerification otpVerification);
}
