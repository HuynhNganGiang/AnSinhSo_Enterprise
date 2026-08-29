using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using AnSinhSo.Domain.Aggregates.PermissionGroupAggregate;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Seeding;

public class PermissionGroupSeeder : IDataSeeder
{
    public int Order => 1;

    public async Task SeedAsync(AnSinhSoDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        var adminGroupCode = "SYSTEM";
        if (!await context.PermissionGroups.AnyAsync(g => g.Code == adminGroupCode, cancellationToken))
        {
            var result = PermissionGroup.Create(
                new PermissionGroupId(Guid.Parse("00000000-0000-0000-0000-000000000001")),
                adminGroupCode,
                "Hệ thống",
                "Quản lý hệ thống"
            );
            if (result.IsSuccess)
            {
                context.PermissionGroups.Add(result.Value);
            }
        }
    }
}
