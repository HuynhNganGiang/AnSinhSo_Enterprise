using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication;
using Microsoft.Extensions.Logging;

namespace AnSinhSo.Infrastructure.Authentication.Providers;

public class OtpDeliveryStrategy : IOtpDeliveryStrategy
{
    private readonly IEnumerable<IOtpProvider> _providers;
    private readonly ILogger<OtpDeliveryStrategy> _logger;

    public OtpDeliveryStrategy(IEnumerable<IOtpProvider> providers, ILogger<OtpDeliveryStrategy> logger)
    {
        _providers = providers.OrderBy(p => p.Priority).ToList();
        _logger = logger;
    }

    public async Task<DeliveryResult> DeliverOtpAsync(string? providerType, string phoneNumber, string otp, CancellationToken cancellationToken = default)
    {
        var startTime = System.Diagnostics.Stopwatch.GetTimestamp();

        foreach (var provider in _providers)
        {
            if (!provider.CanHandle(providerType ?? string.Empty))
            {
                continue;
            }

            try
            {
                var result = await provider.SendOtpAsync(phoneNumber, otp, cancellationToken);
                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully sent OTP via {Provider}", provider.GetType().Name);
                    return result;
                }
                
                _logger.LogWarning("Provider {Provider} failed with error {Error}, falling back to next provider...", provider.GetType().Name, result.ErrorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Provider {Provider} threw an exception, falling back to next provider...", provider.GetType().Name);
            }
        }

        var duration = System.Diagnostics.Stopwatch.GetElapsedTime(startTime);
        _logger.LogError("All OTP providers failed to send OTP to {PhoneNumber}", phoneNumber);
        return DeliveryResult.Failure("All", "AllFailed", "Could not send OTP via any configured providers.", duration);
    }
}
