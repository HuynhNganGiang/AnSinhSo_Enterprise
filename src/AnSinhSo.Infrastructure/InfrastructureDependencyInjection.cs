using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using AnSinhSo.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnSinhSo.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AnSinhSoDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AnSinhSoDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<AnSinhSoDbContext>());
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

        return services;
    }
}
