namespace AnSinhSo.Application.Abstractions.Authentication.RateLimiting;

public interface IRateLimitKeyBuilder
{
    string BuildKey(RateLimitScope scope, string identifier, string windowType);
}
