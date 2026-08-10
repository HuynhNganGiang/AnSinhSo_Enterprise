using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Application.Abstractions.Notifications;

public interface IOtpNotificationService
{
    Task SendOtpAsync(string phoneNumber, string otpCode, string purpose, CancellationToken cancellationToken = default);
}
