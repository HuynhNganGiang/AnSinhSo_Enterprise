using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Notifications;
using Microsoft.Extensions.Logging;

namespace AnSinhSo.Infrastructure.Notifications;

public sealed class ConsoleOtpNotificationService : IOtpNotificationService
{
    private readonly ILogger<ConsoleOtpNotificationService> _logger;

    public ConsoleOtpNotificationService(ILogger<ConsoleOtpNotificationService> logger)
    {
        _logger = logger;
    }

    public Task SendOtpAsync(string phoneNumber, string otpCode, string purpose, CancellationToken cancellationToken = default)
    {
        try
        {
            // Dev environment implementation: Log the OTP to the console instead of sending real SMS
            // WARNING: DO NOT use this in production.
            _logger.LogInformation("CONSOLE SMS SENDER");
            _logger.LogInformation("=========================");
            _logger.LogInformation("To: {PhoneNumber}", phoneNumber);
            _logger.LogInformation("Purpose: {Purpose}", purpose);
            _logger.LogInformation("OTP Code: {OtpCode}", otpCode);
            _logger.LogInformation("=============================================");
        }
        catch (Exception ex)
        {
            // AD #74: Notification Failure Policy
            // Log and swallow the exception, DO NOT rollback the transaction
            _logger.LogError(ex, "Failed to send OTP to {PhoneNumber}", phoneNumber);
        }

        return Task.CompletedTask;
    }
}
