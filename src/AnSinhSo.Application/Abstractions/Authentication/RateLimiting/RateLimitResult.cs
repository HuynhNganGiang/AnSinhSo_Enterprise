namespace AnSinhSo.Application.Abstractions.Authentication.RateLimiting;

public sealed class RateLimitResult
{
    public bool Allowed { get; init; }
    public int RetryAfterSeconds { get; init; }
    public string Reason { get; init; } = string.Empty;
    public RateLimitType Type { get; init; }
    public RateLimitScope Scope { get; init; }
    public int Limit { get; init; }
    public int Remaining { get; init; }
    public long ResetUnixTimeSeconds { get; init; }
    
    public static RateLimitResult Success(int limit, int remaining, long resetUnixTimeSeconds) => new()
    {
        Allowed = true,
        Limit = limit,
        Remaining = remaining,
        ResetUnixTimeSeconds = resetUnixTimeSeconds,
        Type = RateLimitType.None,
        Scope = RateLimitScope.Global
    };
    
    public static RateLimitResult Blocked(RateLimitType type, RateLimitScope scope, string reason, int retryAfterSeconds, int limit, long resetUnixTimeSeconds) => new()
    {
        Allowed = false,
        Type = type,
        Scope = scope,
        Reason = reason,
        RetryAfterSeconds = retryAfterSeconds,
        Limit = limit,
        Remaining = 0,
        ResetUnixTimeSeconds = resetUnixTimeSeconds
    };
}
