using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AnSinhSo.Infrastructure.Persistence.Contexts;

public class AnSinhSoDbContextFactory : IDesignTimeDbContextFactory<AnSinhSoDbContext>
{
    public AnSinhSoDbContext CreateDbContext(string[] args)
    {
        var apiPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "AnSinhSo.API");
        var basePath = Directory.Exists(apiPath) ? apiPath : Directory.GetCurrentDirectory();

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var builder = new DbContextOptionsBuilder<AnSinhSoDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(AnSinhSoDbContext).Assembly.FullName));

        return new AnSinhSoDbContext(builder.Options);
    }
}
