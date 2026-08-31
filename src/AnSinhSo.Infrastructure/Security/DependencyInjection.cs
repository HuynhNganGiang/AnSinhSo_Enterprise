
using Microsoft.Extensions.DependencyInjection;
using AnSinhSo.Application.Common.Security;
using AnSinhSo.Infrastructure.Security.Authentication;
using AnSinhSo.Infrastructure.Security.Identity;
using System;

namespace AnSinhSo.Infrastructure;

public static class SecurityDependencyInjection
{
    public static IServiceCollection AddSecurityInfrastructure(this IServiceCollection services)
    {
        // 1. Core Services
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        services.AddScoped<IClientInfoProvider, ClientInfoProvider>();
        services.AddSingleton<AnSinhSo.Application.Common.Interfaces.Security.ISecurityStampGenerator, SecurityStampGenerator>();

        // 2. HttpContext Accessor (required for CurrentUserProvider)
        services.AddHttpContextAccessor();

        // 3. Options

        return services;
    }
}
