using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Seeding;

public class RoleSeeder : IDataSeeder
{
    public int Order => 3;

    public static readonly Guid AdminRoleId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    public static readonly Guid OfficerRoleId = Guid.Parse("00000000-0000-0000-0000-000000000002");
    public static readonly Guid CitizenRoleId = Guid.Parse("00000000-0000-0000-0000-000000000003");

    public async Task SeedAsync(AnSinhSoDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        await SeedRoleAsync(context, AdminRoleId, "Admin", "Quản trị viên hệ thống", true, cancellationToken);
        await SeedRoleAsync(context, OfficerRoleId, "Officer", "Cán bộ nghiệp vụ", true, cancellationToken);
        await SeedRoleAsync(context, CitizenRoleId, "Citizen", "Công dân", true, cancellationToken);
    }

    private async Task SeedRoleAsync(AnSinhSoDbContext context, Guid roleId, string name, string description, bool isSystem, CancellationToken cancellationToken)
    {
        if (!await context.Roles.AnyAsync(r => r.Name == name, cancellationToken))
        {
            var roleResult = Role.Create(new RoleId(roleId), name, description, isSystem);
            if (roleResult.IsSuccess)
            {
                context.Roles.Add(roleResult.Value);
            }
        }
    }
}
