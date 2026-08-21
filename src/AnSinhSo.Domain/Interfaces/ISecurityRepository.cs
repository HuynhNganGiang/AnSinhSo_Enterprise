using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.SecurityAggregate;

namespace AnSinhSo.Domain.Interfaces;

public interface ISecurityRepository
{
    void AddAuditLogin(AuditLogin auditLogin);
    void AddLoginHistory(LoginHistory loginHistory);
    void AddSecurityLog(SecurityLog securityLog);
    void AddDeviceSession(DeviceSession deviceSession);
    void AddRefreshToken(RefreshToken refreshToken);
    
    Task<DeviceSession?> GetDeviceSessionByIdAsync(AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects.DeviceSessionId id, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetRefreshTokenAsync(System.Guid id, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetRefreshTokenByHashAsync(string hash, CancellationToken cancellationToken = default);
    Task<System.Collections.Generic.List<RefreshToken>> GetRefreshTokensByFamilyAsync(System.Guid familyId, CancellationToken cancellationToken = default);
    Task<System.Collections.Generic.List<DeviceSession>> GetActiveDeviceSessionsByUserIdAsync(System.Guid userId, CancellationToken cancellationToken = default);
    
    void UpdateDeviceSession(DeviceSession deviceSession);
    void UpdateRefreshToken(RefreshToken refreshToken);
}
