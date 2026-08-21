using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Infrastructure.Services.Sms;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AnSinhSo.Infrastructure.Authentication.Providers;

public class SmsOtpProvider : IOtpProvider
{
    private readonly HttpClient _httpClient;
    private readonly SmsOptions _options;
    private readonly ILogger<SmsOtpProvider> _logger;

    public int Priority => 2;

    public SmsOtpProvider(HttpClient httpClient, IOptions<SmsOptions> options, ILogger<SmsOtpProvider> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public bool CanHandle(string providerType)
    {
        return string.IsNullOrEmpty(providerType) || providerType.Equals("Sms", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<DeliveryResult> SendOtpAsync(string phoneNumber, string otp, CancellationToken cancellationToken = default)
    {
        var startTime = System.Diagnostics.Stopwatch.GetTimestamp();

        try
        {
            _logger.LogInformation("Sending OTP via SMS to {PhoneNumber}", phoneNumber);

            var payload = new
            {
                ApiKey = _options.ApiKey,
                To = phoneNumber,
                Message = $"Your AnSinhSo verification code is {otp}",
                SenderId = _options.SenderId
            };

            var response = await _httpClient.PostAsJsonAsync(_options.ApiUrl, payload, cancellationToken);
            var duration = System.Diagnostics.Stopwatch.GetElapsedTime(startTime);

            if (response.IsSuccessStatusCode)
            {
                return DeliveryResult.Success("Sms", null, duration);
            }
            
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return DeliveryResult.Failure("Sms", response.StatusCode.ToString(), errorContent, duration);
        }
        catch (Exception ex)
        {
            var duration = System.Diagnostics.Stopwatch.GetElapsedTime(startTime);
            _logger.LogError(ex, "Failed to send OTP via SMS");
            return DeliveryResult.Failure("Sms", "Exception", ex.Message, duration);
        }
    }
}
