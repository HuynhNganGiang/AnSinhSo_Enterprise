using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace AnSinhSo.Api.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
                policy.RequireClaim(ClaimTypes.Role, "Admin"));
                
            options.AddPolicy("Staff", policy =>
                policy.RequireClaim(ClaimTypes.Role, "Staff", "Admin"));
                
            options.AddPolicy("User", policy =>
                policy.RequireClaim(ClaimTypes.Role, "User", "Staff", "Admin"));
        });

        return services;
    }
}
