using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate.ValueObjects;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public sealed class OtpVerificationRepository : IOtpVerificationRepository
{
    private readonly AnSinhSoDbContext _context;

    public OtpVerificationRepository(AnSinhSoDbContext context)
    {
        _context = context;
    }

    public async Task<OtpVerification?> GetByIdAsync(OtpVerificationId id, CancellationToken cancellationToken = default)
    {
        return await _context.OtpVerifications
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<OtpVerification?> GetPendingByCitizenIdentityIdAsync(CitizenIdentityId citizenIdentityId, CancellationToken cancellationToken = default)
    {
        return await _context.OtpVerifications
            .FirstOrDefaultAsync(o => o.CitizenIdentityId == citizenIdentityId && o.Status == OtpStatus.Pending, cancellationToken);
    }

    public async Task<OtpVerification?> GetByRequestIdAsync(System.Guid requestId, CancellationToken cancellationToken = default)
    {
        return await _context.OtpVerifications
            .FirstOrDefaultAsync(o => o.RequestId == requestId, cancellationToken);
    }

    public void Add(OtpVerification otpVerification)
    {
        _context.OtpVerifications.Add(otpVerification);
    }

    public void Update(OtpVerification otpVerification)
    {
        _context.OtpVerifications.Update(otpVerification);
    }
}
