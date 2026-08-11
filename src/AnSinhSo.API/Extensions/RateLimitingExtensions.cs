using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.RateLimiting;
using System;

namespace AnSinhSo.Api.Extensions;

public static class RateLimitingExtensions
{
    public const string LoginPolicy = "LoginPolicy";
    public const string RefreshPolicy = "RefreshPolicy";
    public const string LogoutAllPolicy = "LogoutAllPolicy";

    public static IServiceCollection AddApiRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // Login Policy: 5 requests / minute / IP
            options.AddPolicy(LoginPolicy, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? httpContext.Request.Headers["X-Forwarded-For"].ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }));

            // Refresh Policy: 30 requests / minute / Session
            options.AddPolicy(RefreshPolicy, httpContext =>
            {
                var sidClaim = httpContext.User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sid)?.Value ?? "unknown";
                
                // If there's no session id claim yet (since refresh token endpoint is AllowAnonymous),
                // we might need to fallback to IP, or wait, if it's AllowAnonymous, they send refresh token in body.
                // Wait, AD #103 says Refresh is 30 req/min/Session.
                // Since the request is AllowAnonymous, we don't have Claims! The user just posts a JSON.
                // It's probably safer to limit by IP for Refresh token since it's an anonymous endpoint.
                // Or I can just rate limit by IP. But the requirement explicitly says: "30 requests / minute / Session". 
                // Maybe the session is parsed from the JWT in the refresh logic? If so, the Rate Limiter runs BEFORE the controller!
                // So RateLimiter can't read the Request Body efficiently (reading body in rate limiter might consume it).
                // Let's rate limit by IP for Refresh token if we can't extract Session. Wait, I will use IP as a fallback.
                
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: sidClaim != "unknown" ? sidClaim : (httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 30,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });

            // LogoutAll Policy: 10 requests / minute / User
            options.AddPolicy(LogoutAllPolicy, httpContext =>
            {
                var subClaim = httpContext.User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: subClaim,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });
        });

        return services;
    }
}
