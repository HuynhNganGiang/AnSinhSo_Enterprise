using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.SecurityAggregate;
using AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public class SecurityRepository : ISecurityRepository
{
    private readonly AnSinhSoDbContext _dbContext;

    public SecurityRepository(AnSinhSoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddAuditLogin(AuditLogin auditLogin)
    {
        _dbContext.Set<AuditLogin>().Add(auditLogin);
    }

    public void AddLoginHistory(LoginHistory loginHistory)
    {
        _dbContext.Set<LoginHistory>().Add(loginHistory);
    }

    public void AddSecurityLog(SecurityLog securityLog)
    {
        _dbContext.Set<SecurityLog>().Add(securityLog);
    }

    public void AddDeviceSession(DeviceSession deviceSession)
    {
        _dbContext.Set<DeviceSession>().Add(deviceSession);
    }

    public void AddRefreshToken(RefreshToken refreshToken)
    {
        _dbContext.Set<RefreshToken>().Add(refreshToken);
    }

    public async Task<DeviceSession?> GetDeviceSessionByIdAsync(DeviceSessionId id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<DeviceSession>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(System.Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<RefreshToken>().FirstOrDefaultAsync(x => x.Id.Value == id, cancellationToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenByHashAsync(string hash, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<RefreshToken>().FirstOrDefaultAsync(x => x.TokenHash == hash, cancellationToken);
    }

    public async Task<System.Collections.Generic.List<RefreshToken>> GetRefreshTokensByFamilyAsync(System.Guid familyId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<RefreshToken>().Where(x => x.FamilyId == familyId).ToListAsync(cancellationToken);
    }

    public async Task<System.Collections.Generic.List<DeviceSession>> GetActiveDeviceSessionsByUserIdAsync(System.Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<DeviceSession>()
            .Where(x => x.UserId == userId && x.RevokedAt == null)
            .ToListAsync(cancellationToken);
    }

    public async Task<System.Collections.Generic.List<DeviceSession>> GetSessionsForArchivalAsync(
        System.DateTime inactiveBefore,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<DeviceSession>()
            .Where(x => !x.IsArchived &&
                (x.RevokedAt != null || x.LastSeenAt < inactiveBefore))
            .ToListAsync(cancellationToken);
    }

    public void UpdateDeviceSession(DeviceSession deviceSession)
    {
        _dbContext.Set<DeviceSession>().Update(deviceSession);
    }

    public void UpdateRefreshToken(RefreshToken refreshToken)
    {
        _dbContext.Set<RefreshToken>().Update(refreshToken);
    }
}
