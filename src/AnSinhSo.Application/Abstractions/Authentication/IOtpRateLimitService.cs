using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication.RateLimiting;

namespace AnSinhSo.Application.Abstractions.Authentication;

public interface IOtpRateLimitService
{
    Task<RateLimitResult> CheckRateLimitAsync(string phoneNumber, string ip, string deviceFingerprint, CancellationToken cancellationToken = default);
    Task RecordSuccessAsync(string phoneNumber, string ip, string deviceFingerprint, CancellationToken cancellationToken = default);
}
