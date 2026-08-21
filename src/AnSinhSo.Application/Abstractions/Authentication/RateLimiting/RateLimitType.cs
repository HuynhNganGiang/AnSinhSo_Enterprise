namespace AnSinhSo.Application.Abstractions.Authentication.RateLimiting;

public enum RateLimitType
{
    None,
    Cooldown,
    HourlyLimit,
    DailyLimit,
    IpBlocked,
    DeviceBlocked
}
