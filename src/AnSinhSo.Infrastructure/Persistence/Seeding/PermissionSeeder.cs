using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using AnSinhSo.Domain.Aggregates.PermissionAggregate;
using AnSinhSo.Domain.Aggregates.PermissionGroupAggregate;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Seeding;

public class PermissionSeeder : IDataSeeder
{
    public int Order => 2;

    public async Task SeedAsync(
        AnSinhSoDbContext context,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        var systemGroup = await context.PermissionGroups
            .FirstOrDefaultAsync(
                g => g.Code == "SYSTEM",
                cancellationToken);

        if (systemGroup == null)
        {
            throw new InvalidOperationException(
                "Permission group SYSTEM was not found.");
        }

        var permissions = new[]
        {
            ("system.login", "Đăng nhập hệ thống"),

            ("roles.view", "Xem chức danh"),
            ("roles.create", "Tạo chức danh"),
            ("roles.update", "Cập nhật chức danh"),
            ("roles.delete", "Xóa chức danh"),

            ("permissions.view", "Xem quyền hạn"),
            ("permissions.manage", "Quản lý quyền hạn"),

            ("userroles.view", "Xem phân quyền người dùng"),
            ("userroles.manage", "Quản lý phân quyền người dùng"),

            ("citizens.view", "Xem công dân"),
            ("citizens.create", "Tạo công dân"),
            ("citizens.update", "Cập nhật công dân"),
            ("citizens.activate", "Kích hoạt công dân"),
            ("citizens.deactivate", "Ngừng kích hoạt công dân"),
            ("citizens.delete", "Xóa công dân"),

            ("households.read", "Xem hộ gia đình"),
            ("households.create", "Tạo hộ gia đình"),
            ("households.update", "Cập nhật hộ gia đình"),
            ("households.delete", "Xóa hộ gia đình"),

            ("payments.view", "Xem chi trả"),
            ("payments.create", "Tạo chi trả"),
            ("payments.approve", "Phê duyệt chi trả"),
            ("payments.complete", "Hoàn tất chi trả"),
            ("payments.cancel", "Hủy chi trả"),

            ("gis.read", "Xem dữ liệu bản đồ"),
            ("gis.create", "Tạo dữ liệu bản đồ"),
            ("gis.update", "Cập nhật dữ liệu bản đồ"),
            ("gis.delete", "Xóa dữ liệu bản đồ"),

            ("notifications.view", "Xem thông báo"),
            ("notifications.create", "Tạo thông báo"),
            ("notifications.broadcast", "Gửi thông báo"),
            ("notifications.retry", "Gửi lại thông báo"),
            ("notifications.read", "Đọc thông báo"),
            ("notifications.statistics", "Xem thống kê thông báo"),

            ("ai.analyze", "Phân tích AI"),
            ("ai.scan", "Quét dữ liệu AI"),
            ("ai.viewrecommendations", "Xem khuyến nghị AI"),
            ("ai.updatestatus", "Cập nhật trạng thái AI"),
            ("ai.dashboard", "Xem Dashboard AI"),

            ("welfareprograms.view", "Xem chương trình an sinh"),
            ("welfareprograms.create", "Tạo chương trình an sinh"),
            ("welfareprograms.update", "Cập nhật chương trình an sinh"),

            ("welfarecases.view", "Xem hồ sơ an sinh"),
            ("welfarecases.create", "Tạo hồ sơ an sinh"),
            ("welfarecases.update", "Cập nhật hồ sơ an sinh"),
            ("welfarecases.decide", "Ra quyết định hồ sơ an sinh"),
            ("welfarecases.cancel", "Hủy hồ sơ an sinh"),
            ("welfarecases.close", "Đóng hồ sơ an sinh"),

            ("map.view", "Xem bản đồ"),
            ("map.updatelocation", "Cập nhật vị trí bản đồ")
        };

        foreach (var item in permissions)
        {
            var exists = await context.Permissions
                .AnyAsync(
                    p => p.Code == item.Item1,
                    cancellationToken);

            if (exists)
            {
                continue;
            }

            var result = Permission.Create(
                new PermissionId(Guid.NewGuid()),
                item.Item1,
                item.Item2,
                item.Item2,
                systemGroup.Id);

            if (result.IsSuccess)
            {
                context.Permissions.Add(result.Value);
            }
        }
    }
}