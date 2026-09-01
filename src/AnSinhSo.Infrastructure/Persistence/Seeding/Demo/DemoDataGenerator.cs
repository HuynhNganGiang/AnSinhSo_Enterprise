using System;
using System.Collections.Generic;
using System.Linq;

namespace AnSinhSo.Infrastructure.Persistence.Seeding.Demo;

public static class DemoDataGenerator
{
    private static readonly Random _random = new Random(12345); // Fixed seed for reproducible results

    private static readonly string[] _lastNames = { "Nguyễn", "Trần", "Lê", "Phạm", "Hoàng", "Huỳnh", "Phan", "Vũ", "Võ", "Đặng", "Bùi", "Đỗ", "Hồ", "Ngô", "Dương", "Lý" };

    private static readonly string[] _maleMiddleNames = { "Văn", "Hữu", "Đức", "Công", "Minh", "Quang", "Bảo", "Đình", "Thái", "Tuấn", "Hoàng", "Ngọc" };
    private static readonly string[] _femaleMiddleNames = { "Thị", "Ngọc", "Thu", "Phương", "Mỹ", "Thanh", "Bích", "Tuyết", "Hồng", "Kim" };

    private static readonly string[] _maleFirstNames = { "Dũng", "Anh", "Tuấn", "Nam", "Bình", "Sơn", "Long", "Hiếu", "Phát", "Đạt", "Thắng", "Cường", "Tài", "Hải", "Phong" };
    private static readonly string[] _femaleFirstNames = { "Lan", "Hương", "Trang", "Linh", "Hoa", "Mai", "Nhung", "Thảo", "Hà", "Yến", "Ngân", "Giang", "Quyên", "Vy" };

    private static readonly string[] _streets = { "Thôn 1", "Thôn 2", "Thôn Suối Nhuôm", "Thôn Hòa Bình", "Thôn Tú Sơn", "Thôn Sông Khiêng", "Thôn Bình Sơn", "Thôn Bình Nghĩa" };
    private static readonly string[] _wards = { "Xã Sông Lũy" };
    private static readonly string[] _districts = { "Bắc Bình" };
    private static readonly string[] _provinces = { "Lâm Đồng" };

    public static string GenerateMaleName()
    {
        var last = _lastNames[_random.Next(_lastNames.Length)];
        var middle = _maleMiddleNames[_random.Next(_maleMiddleNames.Length)];
        var first = _maleFirstNames[_random.Next(_maleFirstNames.Length)];
        return $"{last} {middle} {first}";
    }

    public static string GenerateFemaleName()
    {
        var last = _lastNames[_random.Next(_lastNames.Length)];
        var middle = _femaleMiddleNames[_random.Next(_femaleMiddleNames.Length)];
        var first = _femaleFirstNames[_random.Next(_femaleFirstNames.Length)];
        return $"{last} {middle} {first}";
    }

    public static string GenerateName(bool isMale)
    {
        return isMale ? GenerateMaleName() : GenerateFemaleName();
    }

    public static string GenerateAddress()
    {
        var number = _random.Next(1, 999);
        var street = _streets[_random.Next(_streets.Length)];
        var ward = _wards[_random.Next(_wards.Length)];
        var district = _districts[_random.Next(_districts.Length)];
        var province = _provinces[0];
        return $"{number} {street}, {ward}, {district}, {province}";
    }

    public static string GeneratePhoneNumber()
    {
        return $"0{_random.Next(8, 10)}{_random.Next(10000000, 99999999)}";
    }

    public static string GenerateCitizenId()
    {
        return $"0{_random.Next(10, 99)}{_random.Next(100, 399)}{_random.Next(100000, 999999)}";
    }

    public static DateTime GenerateBirthDate(int minAge, int maxAge)
    {
        var today = DateTime.UtcNow;
        var start = today.AddYears(-maxAge);
        var end = today.AddYears(-minAge);
        var range = (end - start).Days;
        return start.AddDays(_random.Next(range));
    }

    public static T? PickRandom<T>(IList<T> items)
    {
        if (items == null || items.Count == 0) return default;
        return items[_random.Next(items.Count)];
    }

    public static List<T> PickRandomItems<T>(IList<T> items, int count)
    {
        if (items == null || items.Count == 0 || count <= 0) return new List<T>();
        return items.OrderBy(x => _random.Next()).Take(count).ToList();
    }

    public static bool NextBool(double trueProbability = 0.5)
    {
        return _random.NextDouble() < trueProbability;
    }
}
