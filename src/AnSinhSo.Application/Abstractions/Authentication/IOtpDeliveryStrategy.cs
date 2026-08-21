using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Application.Abstractions.Authentication;

public interface IOtpDeliveryStrategy
{
    Task<DeliveryResult> DeliverOtpAsync(string? providerType, string phoneNumber, string otp, CancellationToken cancellationToken = default);
}
