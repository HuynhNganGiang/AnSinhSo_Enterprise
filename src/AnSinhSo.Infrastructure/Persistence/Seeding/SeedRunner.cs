using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnSinhSo.Infrastructure.Persistence.Seeding;

public class SeedRunner
{
    private readonly AnSinhSoDbContext _context;
    private readonly IEnumerable<IDataSeeder> _seeders;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SeedRunner> _logger;

    public SeedRunner(
        AnSinhSoDbContext context,
        IEnumerable<IDataSeeder> seeders,
        IServiceProvider serviceProvider,
        ILogger<SeedRunner> logger)
    {
        _context = context;
        _seeders = seeders.OrderBy(s => s.Order);
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting database seeding...");

        foreach(var seeder in _seeders)
        {
            try
            {
                _logger.LogInformation($"Running seeder: {seeder.GetType().Name}");
                await seeder.SeedAsync(_context, _serviceProvider, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while running seeder {seeder.GetType().Name}");
                throw;
            }
        }

        _logger.LogInformation("Database seeding completed.");
    }
}
