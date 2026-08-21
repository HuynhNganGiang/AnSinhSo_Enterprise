using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Infrastructure.Services.Zalo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AnSinhSo.Infrastructure.Authentication.Providers;

public class ZaloOtpProvider : IOtpProvider
{
    private readonly HttpClient _httpClient;
    private readonly ZaloOptions _options;
    private readonly ILogger<ZaloOtpProvider> _logger;

    public int Priority => 1;

    public ZaloOtpProvider(HttpClient httpClient, IOptions<ZaloOptions> options, ILogger<ZaloOtpProvider> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public bool CanHandle(string providerType)
    {
        return string.IsNullOrEmpty(providerType) || providerType.Equals("Zalo", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<DeliveryResult> SendOtpAsync(string phoneNumber, string otp, CancellationToken cancellationToken = default)
    {
        var startTime = System.Diagnostics.Stopwatch.GetTimestamp();
        
        try
        {
            _logger.LogInformation("Sending OTP via Zalo to {PhoneNumber}", phoneNumber);
            
            var payload = new
            {
                recipient = new { phone = phoneNumber },
                message = new { text = $"Your OTP is {otp}" }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://openapi.zalo.me/v3.0/oa/message/cs");
            request.Headers.Add("access_token", _options.AccessToken);
            request.Content = JsonContent.Create(payload);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            var duration = System.Diagnostics.Stopwatch.GetElapsedTime(startTime);
            
            if (response.IsSuccessStatusCode)
            {
                return DeliveryResult.Success("Zalo", null, duration);
            }
            
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return DeliveryResult.Failure("Zalo", response.StatusCode.ToString(), errorContent, duration);
        }
        catch (Exception ex)
        {
            var duration = System.Diagnostics.Stopwatch.GetElapsedTime(startTime);
            _logger.LogError(ex, "Failed to send OTP via Zalo");
            return DeliveryResult.Failure("Zalo", "Exception", ex.Message, duration);
        }
    }
}
