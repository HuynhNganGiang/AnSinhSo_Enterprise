using System;

namespace AnSinhSo.Application.Abstractions.Authentication;

public class DeliveryResult
{
    public bool IsSuccess { get; init; }
    public string Provider { get; init; } = string.Empty;
    public string? ProviderMessageId { get; init; }
    public string? ErrorCode { get; init; }
    public string? ErrorMessage { get; init; }
    public TimeSpan Duration { get; init; }
    public DateTime SentAt { get; init; }
    
    public static DeliveryResult Success(string provider, string? providerMessageId, TimeSpan duration)
    {
        return new DeliveryResult
        {
            IsSuccess = true,
            Provider = provider,
            ProviderMessageId = providerMessageId,
            Duration = duration,
            SentAt = DateTime.UtcNow
        };
    }
    
    public static DeliveryResult Failure(string provider, string? errorCode, string? errorMessage, TimeSpan duration)
    {
        return new DeliveryResult
        {
            IsSuccess = false,
            Provider = provider,
            ErrorCode = errorCode,
            ErrorMessage = errorMessage,
            Duration = duration,
            SentAt = DateTime.UtcNow
        };
    }
}
