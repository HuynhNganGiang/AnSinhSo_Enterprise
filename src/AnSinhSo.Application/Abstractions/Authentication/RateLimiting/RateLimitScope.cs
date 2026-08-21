namespace AnSinhSo.Application.Abstractions.Authentication.RateLimiting;

public enum RateLimitScope
{
    Phone,
    Device,
    IP,
    User,
    Citizen,
    Global
}
