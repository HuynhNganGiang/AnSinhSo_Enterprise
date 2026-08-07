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

        // Domain UoW
        services.AddScoped<AnSinhSo.Domain.Interfaces.IUnitOfWork, UnitOfWork>();

        // Specialized Repositories (Domain)
        services.AddScoped<AnSinhSo.Domain.Aggregates.CitizenAggregate.ICitizenRepository, CitizenRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.HouseholdAggregate.IHouseholdRepository, HouseholdRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.PaymentAggregate.IPaymentRepository, PaymentRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.PolicyAggregate.IPolicyRepository, PolicyRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.WelfareGroupAggregate.IWelfareGroupRepository, WelfareGroupRepository>();

        return services;
    }
}
