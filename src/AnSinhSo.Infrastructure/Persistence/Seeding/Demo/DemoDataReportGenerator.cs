using System.IO;

namespace AnSinhSo.Infrastructure.Persistence.Seeding.Demo;

public static class DemoDataReportGenerator
{
    public static void GenerateReport(DemoDataOptions options)
    {
        const string filePath = "DemoDataReport.md";

        string content = $@"# Báo cáo dữ liệu Demo doanh nghiệp

## Cấu hình

- Kích hoạt: {options.Enabled}
- Mục tiêu số hộ gia đình: {options.Households}

## Chuỗi dữ liệu nghiệp vụ được sinh

Demo Seeder sử dụng số hộ gia đình đã cấu hình làm mục tiêu sinh dữ liệu chính.

Với mỗi hộ gia đình, Seeder tạo dữ liệu hộ gia đình và công dân cốt lõi cần thiết cho kịch bản Demo.

Tùy theo quy tắc nghiệp vụ và xác suất sinh dữ liệu, các bản ghi phụ thuộc có thể bao gồm:

- Hồ sơ an sinh
- Chi trả
- Thông báo
- Nhật ký đăng nhập
- Nhật ký an ninh
- Khuyến nghị AI

Số lượng các bản ghi phụ thuộc này không được cấu hình độc lập và không được đảm bảo theo một số lượng cố định.

## An toàn

Chức năng sinh dữ liệu Demo bị tắt theo mặc định.

Chỉ kích hoạt trong môi trường được phép sử dụng dữ liệu Demo tổng hợp.
";

        File.WriteAllText(filePath, content);
    }
}