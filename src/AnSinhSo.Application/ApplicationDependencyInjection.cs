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

        return services;
    }
}
