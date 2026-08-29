using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace AnSinhSo.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(AnSinhSo.Application.Common.Behaviors.UnhandledExceptionBehavior<,>));
            cfg.AddOpenBehavior(typeof(AnSinhSo.Application.Common.Behaviors.LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(AnSinhSo.Application.Common.Behaviors.ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);
        services.AddAutoMapper(cfg => cfg.AddProfile<AnSinhSo.Application.Common.Mappings.MappingProfile>());

        services.AddScoped<AnSinhSo.Application.Abstractions.Authentication.IOtpVerificationService, AnSinhSo.Application.Authentication.Services.OtpVerificationService>();
        services.AddScoped<AnSinhSo.Application.Abstractions.Authentication.IIdentityVerificationService, AnSinhSo.Application.Authentication.Services.IdentityVerificationService>();
        services.AddScoped<AnSinhSo.Application.Abstractions.Authentication.ITokenIssuingService, AnSinhSo.Application.Authentication.Services.TokenIssuingService>();
        services.AddScoped<AnSinhSo.Application.Abstractions.Authentication.ISecurityAuditService, AnSinhSo.Application.Authentication.Services.SecurityAuditService>();

        services.AddScoped<AnSinhSo.Application.Authentication.BackOffice.RefreshToken.Resolvers.IRefreshSessionResolver, AnSinhSo.Application.Authentication.BackOffice.RefreshToken.Resolvers.SecurityRefreshResolver>();
        return services;
    }
}
