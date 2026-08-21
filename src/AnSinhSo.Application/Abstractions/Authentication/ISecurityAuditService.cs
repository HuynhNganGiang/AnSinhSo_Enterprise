using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Application.Abstractions.Authentication;

public interface ISecurityAuditService
{
    Task LogSuccessAsync(Guid userId, string ipAddress, string userAgent, string deviceName, string timeZone, CancellationToken cancellationToken = default);
    Task LogFailureAsync(Guid userId, string ipAddress, string userAgent, string timeZone, string reason, CancellationToken cancellationToken = default);
    Task LogRateLimitExceededAsync(string target, string ipAddress, string deviceFingerprint, string reason, CancellationToken cancellationToken = default);
}
