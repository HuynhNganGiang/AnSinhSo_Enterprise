using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.UserRoleAggregate;
using AnSinhSo.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AnSinhSo.Infrastructure.Persistence.Seeding;

public class UserRoleSeeder : IDataSeeder
{
    public int Order => 6;

    public async Task SeedAsync(
        AnSinhSoDbContext context,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        var options = serviceProvider
            .GetService<IOptions<SeedAdminOptions>>()?.Value;

        if (options == null || string.IsNullOrEmpty(options.Username))
        {
            return;
        }

        var adminUser = await context.Users
            .FirstOrDefaultAsync(
                u => u.Username == options.Username,
                cancellationToken);

        var adminRole = await context.Roles
            .FirstOrDefaultAsync(
                r => r.Name == "Admin",
                cancellationToken);

        if (adminUser == null || adminRole == null)
        {
            return;
        }

        // ---------------------------------------------------------
        // Admin User ID được dùng đồng nhất cho:
        // User -> Citizen -> CitizenIdentity -> UserRole
        // ---------------------------------------------------------
        var adminGuid = adminUser.Id.Value;

        var citizenId = new CitizenId(adminGuid);
        var citizenIdentityId = new CitizenIdentityId(adminGuid);

        // =========================================================
        // 1. Đảm bảo Citizen profile của Admin tồn tại
        // =========================================================
        var citizenExists = await context.Citizens
            .AnyAsync(c => c.Id == citizenId, cancellationToken);

        if (!citizenExists)
        {
            var fullNameResult = FullName.Create(
                "Quản trị",
                "Hệ thống",
                "Admin");

            // CCCD seed dành riêng cho system admin.
            // Rule hiện tại: đúng 12 chữ số.
            var citizenNumberResult =
                CitizenNumber.Create("000000000001");

            // SĐT seed 10 chữ số.
            var phoneNumberResult =
                PhoneNumber.Create("0900000001");

            // Ưu tiên email thật của tài khoản admin.
            var adminEmail = !string.IsNullOrWhiteSpace(adminUser.Email)
                ? adminUser.Email
                : "admin@ansinhso.local";

            var emailResult = Email.Create(adminEmail);

            var postalCodeResult =
                PostalCode.Create("00000");

            if (fullNameResult.IsFailure ||
                citizenNumberResult.IsFailure ||
                phoneNumberResult.IsFailure ||
                emailResult.IsFailure ||
                postalCodeResult.IsFailure)
            {
                throw new InvalidOperationException(
                    "Không thể tạo Value Object cho Citizen profile của Admin.");
            }

            var addressResult = Address.Create(
                "Trụ sở UBND xã Sông Lũy",
                "Sông Lũy",
                "Bắc Bình",
                "Bình Thuận",
                postalCodeResult.Value);

            if (addressResult.IsFailure)
            {
                throw new InvalidOperationException(
                    "Không thể tạo Address cho Citizen profile của Admin.");
            }

            var gender = AnSinhSo.Domain.Enumerations.Enumeration
                .GetAll<Gender>()
                .FirstOrDefault(x => x.Id == Gender.Other.Id)
                ?? Gender.Other;

            var citizenResult = Citizen.Create(
                citizenId,
                fullNameResult.Value,
                citizenNumberResult.Value,
                new DateTime(1990, 1, 1),
                gender,
                phoneNumberResult.Value,
                addressResult.Value,
                emailResult.Value);

            if (citizenResult.IsFailure)
            {
                throw new InvalidOperationException(
                    $"Không thể tạo Citizen profile cho Admin: " +
                    $"{citizenResult.Error.Code} - " +
                    $"{citizenResult.Error.ToString()}");
            }

            context.Citizens.Add(citizenResult.Value);

            // Lưu Citizen trước để bảo đảm FK.
            await context.SaveChangesAsync(cancellationToken);
        }

        // =========================================================
        // 2. Đảm bảo CitizenIdentity tồn tại
        // =========================================================
        var identity = await context
    .Set<CitizenIdentity>()
    .FirstOrDefaultAsync(
        ci => ci.Id == citizenIdentityId,
        cancellationToken);

if (identity == null)
{
    identity = CitizenIdentity.Create(
        citizenIdentityId,
        citizenId,
        adminUser.SecurityStamp);

    context.Set<CitizenIdentity>().Add(identity);
}

if (identity.Status ==
    AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations.IdentityStatus.Pending)
{
    var identityPhone =
        AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects.PhoneNumber.Create(
            "0900000001");

    identity.VerifyPhoneNumber(
        identityPhone,
        DateTime.UtcNow);
}

await context.SaveChangesAsync(cancellationToken);

        // =========================================================
        // 3. Đảm bảo Admin Role được gán
        // =========================================================
        var userRoleExists = await context.UserRoles
            .AnyAsync(
                ur =>
                    ur.CitizenIdentityId == citizenIdentityId &&
                    ur.RoleId == adminRole.Id,
                cancellationToken);

        if (!userRoleExists)
        {
            var userRoleResult = UserRole.Assign(
                new UserRoleId(Guid.NewGuid()),
                citizenIdentityId,
                adminRole.Id);

            if (userRoleResult.IsSuccess)
            {
                context.UserRoles.Add(userRoleResult.Value);

                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
