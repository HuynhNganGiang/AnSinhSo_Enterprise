using AnSinhSo.Application.Abstractions.Authentication.RateLimiting;

namespace AnSinhSo.Infrastructure.Authentication;

public class RateLimitKeyBuilder : IRateLimitKeyBuilder
{
    private const string Prefix = "RL";

    public string BuildKey(RateLimitScope scope, string identifier, string windowType)
    {
        return $"{Prefix}:{scope}:{identifier}:{windowType}";
    }
}
