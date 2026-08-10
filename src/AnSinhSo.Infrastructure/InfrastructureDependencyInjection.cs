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

        services.AddSecurityInfrastructure();

        // Domain UoW
        services.AddScoped<AnSinhSo.Domain.Interfaces.IUnitOfWork, UnitOfWork>();

        // Specialized Repositories (Domain)
        services.AddScoped<AnSinhSo.Domain.Aggregates.CitizenAggregate.ICitizenRepository, CitizenRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.HouseholdAggregate.IHouseholdRepository, HouseholdRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.PaymentAggregate.IPaymentRepository, PaymentRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.PolicyAggregate.IPolicyRepository, PolicyRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.WelfareGroupAggregate.IWelfareGroupRepository, WelfareGroupRepository>();
        services.AddScoped<AnSinhSo.Domain.Interfaces.IUserRepository, UserRepository>();
        services.AddScoped<AnSinhSo.Domain.Interfaces.IUserSessionRepository, UserSessionRepository>();

        // Data Import Pipeline
        services.AddScoped<AnSinhSo.Application.DataImport.IDataImportService, AnSinhSo.Infrastructure.DataImport.DataImportService>();
        services.AddSingleton<AnSinhSo.Infrastructure.DataImport.Normalization.ICsvStringNormalizer, AnSinhSo.Infrastructure.DataImport.Normalization.CsvStringNormalizer>();
        services.AddScoped<AnSinhSo.Infrastructure.DataImport.Mappers.IWelfareGroupMapper, AnSinhSo.Infrastructure.DataImport.Mappers.WelfareGroupMapper>();
        services.AddScoped<AnSinhSo.Infrastructure.DataImport.Mappers.ICitizenMapper, AnSinhSo.Infrastructure.DataImport.Mappers.CitizenMapper>();
        services.AddScoped<AnSinhSo.Infrastructure.DataImport.Mappers.IHouseholdMapper, AnSinhSo.Infrastructure.DataImport.Mappers.HouseholdMapper>();
        services.AddScoped<AnSinhSo.Infrastructure.DataImport.Mappers.IPolicyMapper, AnSinhSo.Infrastructure.DataImport.Mappers.PolicyMapper>();
        services.AddScoped<AnSinhSo.Infrastructure.DataImport.Mappers.IPaymentMapper, AnSinhSo.Infrastructure.DataImport.Mappers.PaymentMapper>();

        return services;
    }
}
