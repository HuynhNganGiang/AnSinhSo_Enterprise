using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Infrastructure.Persistence.Contexts;

namespace AnSinhSo.Infrastructure.Persistence.Seeding;

public interface IDataSeeder
{
    int Order { get; }
    Task SeedAsync(AnSinhSoDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
}
