using CsvHelper.Configuration.Attributes;

namespace AnSinhSo.Infrastructure.DataImport.Models;

public class Stg_RoleRecord
{
    [Name("MaRole")] public string? MaRole { get; set; }
    [Name("TenRole")] public string? TenRole { get; set; }
    [Name("GhiChu")] public string? GhiChu { get; set; }
}

public class Stg_UserRecord
{
    [Name("Username")] public string? Username { get; set; }
    [Name("PasswordHash")] public string? PasswordHash { get; set; }
    [Name("HoTen")] public string? HoTen { get; set; }
    [Name("Email")] public string? Email { get; set; }
    [Name("MaRole")] public string? MaRole { get; set; }
    [Name("TrangThai")] public string? TrangThai { get; set; }
    [Name("GhiChu")] public string? GhiChu { get; set; }
}

public class Stg_DiaBanRecord
{
    [Name("MaDiaBan")] public string? MaDiaBan { get; set; }
    [Name("TenDiaBan")] public string? TenDiaBan { get; set; }
    [Name("CapDiaBan")] public string? CapDiaBan { get; set; }
    [Name("GhiChu")] public string? GhiChu { get; set; }
}

public class Stg_NhomDoiTuongRecord
{
    [Name("MaNhom")] public string? MaNhom { get; set; }
    [Name("TenNhomDoiTuong")] public string? TenNhomDoiTuong { get; set; }
    [Name("DiemUuTienAI")] public string? DiemUuTienAI { get; set; }
    [Name("GhiChu")] public string? GhiChu { get; set; }
}

public class Stg_ChinhSachTroCapRecord
{
    [Name("MaChinhSach")] public string? MaChinhSach { get; set; }
    [Name("TenChinhSach")] public string? TenChinhSach { get; set; }
    [Name("MaNhom")] public string? MaNhom { get; set; }
    [Name("TenNhomDoiTuong")] public string? TenNhomDoiTuong { get; set; }
    [Name("MucTroCapMacDinh")] public string? MucTroCapMacDinh { get; set; }
    [Name("GhiChu")] public string? GhiChu { get; set; }
}

public class Stg_DotChiTraRecord
{
    [Name("MaDotChiTra")] public string? MaDotChiTra { get; set; }
    [Name("TenDotChiTra")] public string? TenDotChiTra { get; set; }
    [Name("Thang")] public string? Thang { get; set; }
    [Name("Nam")] public string? Nam { get; set; }
    [Name("NgayBatDau")] public string? NgayBatDau { get; set; }
    [Name("NgayKetThuc")] public string? NgayKetThuc { get; set; }
    [Name("NguonFile")] public string? NguonFile { get; set; }
    [Name("GhiChu")] public string? GhiChu { get; set; }
}

public class Stg_HoGiaDinhRecord
{
    [Name("MaHo")] public string? MaHo { get; set; }
    [Name("TenChuHo")] public string? TenChuHo { get; set; }
    [Name("CCCDChuHo")] public string? CCCDChuHo { get; set; }
    [Name("SoDienThoai")] public string? SoDienThoai { get; set; }
    [Name("DiaChi")] public string? DiaChi { get; set; }
    [Name("TenDiaBan")] public string? TenDiaBan { get; set; }
    [Name("PhanLoaiHo")] public string? PhanLoaiHo { get; set; }
    [Name("SoThanhVien")] public string? SoThanhVien { get; set; }
    [Name("TrangThaiHo")] public string? TrangThaiHo { get; set; }
    [Name("NguonFile")] public string? NguonFile { get; set; }
    [Name("GhiChu")] public string? GhiChu { get; set; }
}

public class Stg_ThanhVienHoGiaDinhRecord
{
    [Name("MaHo")] public string? MaHo { get; set; }
    [Name("HoTenThanhVien")] public string? HoTenThanhVien { get; set; }
    [Name("QuanHeVoiChuHo")] public string? QuanHeVoiChuHo { get; set; }
    [Name("NgaySinhRaw")] public string? NgaySinhRaw { get; set; }
    [Name("NgaySinhISO")] public string? NgaySinhISO { get; set; }
    [Name("LaNgaySinhUocLuong")] public string? LaNgaySinhUocLuong { get; set; }
    [Name("GioiTinh")] public string? GioiTinh { get; set; }
    [Name("CCCD")] public string? CCCD { get; set; }
    [Name("DiaChi")] public string? DiaChi { get; set; }
    [Name("TenDiaBan")] public string? TenDiaBan { get; set; }
    [Name("DanToc")] public string? DanToc { get; set; }
    [Name("PhanLoaiHo")] public string? PhanLoaiHo { get; set; }
    [Name("TrangThai")] public string? TrangThai { get; set; }
    [Name("NguonFile")] public string? NguonFile { get; set; }
    [Name("DongNguon")] public string? DongNguon { get; set; }
    [Name("GhiChu")] public string? GhiChu { get; set; }
}

public class Stg_LichSuHoGiaDinhRecord
{
    [Name("MaHo")] public string? MaHo { get; set; }
    [Name("TenChuHo")] public string? TenChuHo { get; set; }
    [Name("LoaiBienDong")] public string? LoaiBienDong { get; set; }
    [Name("Nam")] public string? Nam { get; set; }
    [Name("TenDiaBan")] public string? TenDiaBan { get; set; }
    [Name("PhanLoaiTruoc")] public string? PhanLoaiTruoc { get; set; }
    [Name("PhanLoaiSau")] public string? PhanLoaiSau { get; set; }
    [Name("NguonFile")] public string? NguonFile { get; set; }
    [Name("GhiChu")] public string? GhiChu { get; set; }
}

public class Stg_DoiTuongAnSinhRecord
{
    [Name("MaDoiTuong")] public string? MaDoiTuong { get; set; }
    [Name("HoTen")] public string? HoTen { get; set; }
    [Name("NgaySinhRaw")] public string? NgaySinhRaw { get; set; }
    [Name("NgaySinhISO")] public string? NgaySinhISO { get; set; }
    [Name("LaNgaySinhUocLuong")] public string? LaNgaySinhUocLuong { get; set; }
    [Name("GioiTinh")] public string? GioiTinh { get; set; }
    [Name("CCCD")] public string? CCCD { get; set; }
    [Name("SoDienThoai")] public string? SoDienThoai { get; set; }
    [Name("DiaChi")] public string? DiaChi { get; set; }
    [Name("TenDiaBan")] public string? TenDiaBan { get; set; }
    [Name("MaDiaBan")] public string? MaDiaBan { get; set; }
    [Name("TenNhomDoiTuong")] public string? TenNhomDoiTuong { get; set; }
    [Name("MaNhom")] public string? MaNhom { get; set; }
    [Name("MaHo")] public string? MaHo { get; set; }
    [Name("Latitude")] public string? Latitude { get; set; }
    [Name("Longitude")] public string? Longitude { get; set; }
    [Name("TrangThaiHuongTroCap")] public string? TrangThaiHuongTroCap { get; set; }
    [Name("NguonFile")] public string? NguonFile { get; set; }
    [Name("DongNguon")] public string? DongNguon { get; set; }
    [Name("GhiChu")] public string? GhiChu { get; set; }
}

public class Stg_ChiTraTroCapRecord
{
    [Name("MaDotChiTra")] public string? MaDotChiTra { get; set; }
    [Name("MaDoiTuong")] public string? MaDoiTuong { get; set; }
    [Name("MaChinhSach")] public string? MaChinhSach { get; set; }
    [Name("SoTienTroCap")] public string? SoTienTroCap { get; set; }
    [Name("SoTienTruyLinh")] public string? SoTienTruyLinh { get; set; }
    [Name("ThangTruocMangSang")] public string? ThangTruocMangSang { get; set; }
    [Name("TongSoTien")] public string? TongSoTien { get; set; }
    [Name("TrangThaiChiTra")] public string? TrangThaiChiTra { get; set; }
    [Name("NgayChiTra")] public string? NgayChiTra { get; set; }
    [Name("HinhThucChiTra")] public string? HinhThucChiTra { get; set; }
    [Name("NguoiKyNhan")] public string? NguoiKyNhan { get; set; }
    [Name("NguonFile")] public string? NguonFile { get; set; }
    [Name("DongNguon")] public string? DongNguon { get; set; }
    [Name("GhiChu")] public string? GhiChu { get; set; }
}
