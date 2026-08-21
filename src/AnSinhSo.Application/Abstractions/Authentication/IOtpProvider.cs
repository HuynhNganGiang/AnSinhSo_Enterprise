using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Application.Abstractions.Authentication;

public interface IOtpProvider
{
    int Priority { get; }
    bool CanHandle(string providerType);
    Task<DeliveryResult> SendOtpAsync(string phoneNumber, string otp, CancellationToken cancellationToken = default);
}
