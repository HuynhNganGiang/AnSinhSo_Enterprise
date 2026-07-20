using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.ValueObjects;

/// <summary>
/// Value Object đại diện cho địa chỉ.
/// </summary>
public sealed class Address : ValueObject
{
    /// <summary>
    /// Đường/Thôn/Xóm.
    /// </summary>
    public string Street { get; }

    /// <summary>
    /// Xã/Phường.
    /// </summary>
    public string Ward { get; }

    /// <summary>
    /// Quận/Huyện.
    /// </summary>
    public string District { get; }

    /// <summary>
    /// Tỉnh/Thành phố.
    /// </summary>
    public string Province { get; }

    /// <summary>
    /// Mã bưu chính.
    /// </summary>
    public PostalCode PostalCode { get; }

    private Address(string street, string ward, string district, string province, PostalCode postalCode)
    {
        Street = street;
        Ward = ward;
        District = district;
        Province = province;
        PostalCode = postalCode;
    }

    /// <summary>
    /// Khởi tạo Address.
    /// </summary>
    public static Result<Address> Create(string street, string ward, string district, string province, PostalCode postalCode)
    {
        street = street?.Trim() ?? string.Empty;
        ward = ward?.Trim() ?? string.Empty;
        district = district?.Trim() ?? string.Empty;
        province = province?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(street) || string.IsNullOrWhiteSpace(ward) || string.IsNullOrWhiteSpace(district) || string.IsNullOrWhiteSpace(province))
        {
            return Result.Failure<Address>(Error.Validation("Address.Incomplete", "Địa chỉ không được để trống các thành phần cơ bản."));
        }

        if (postalCode is null)
        {
            return Result.Failure<Address>(Error.Validation("Address.PostalCodeNull", "Postal code không được để trống."));
        }

        return Result.Success(new Address(street, ward, district, province, postalCode));
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return Ward;
        yield return District;
        yield return Province;
        yield return PostalCode;
    }
}
