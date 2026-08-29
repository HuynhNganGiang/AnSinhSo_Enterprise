using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Seeding;

public class RolePermissionSeeder : IDataSeeder
{
    public int Order => 4;

    public async Task SeedAsync(
        AnSinhSoDbContext context,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        var adminRole = await context.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(
                r => r.Name == "Admin",
                cancellationToken);

        if (adminRole == null)
        {
            throw new InvalidOperationException(
                "Admin role was not found.");
        }

        var allPermissions = await context.Permissions
            .ToListAsync(cancellationToken);

        var wasSystemRole = adminRole.IsSystemRole;

        if (wasSystemRole)
        {
            adminRole.MarkAsCustom();
        }

        foreach (var permission in allPermissions)
        {
            var alreadyAssigned = adminRole.Permissions
                .Any(rp => rp.PermissionId == permission.Id);

            if (alreadyAssigned)
            {
                continue;
            }

            var result = adminRole.AddPermission(permission.Id);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(
                    $"Cannot assign permission '{permission.Code}' to Admin.");
            }
        }

        if (wasSystemRole)
        {
            adminRole.MarkAsSystem();
        }

        context.Roles.Update(adminRole);
    }
}