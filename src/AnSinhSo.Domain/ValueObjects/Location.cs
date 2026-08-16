using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.ValueObjects;

/// <summary>
/// Value Object đại diện cho toạ độ địa lý trên bản đồ.
/// </summary>
public sealed class Location : ValueObject
{
    public double Latitude { get; }
    public double Longitude { get; }

#pragma warning disable CS8618
    /// <summary>
    /// Constructor rỗng dành riêng cho EF Core.
    /// </summary>
    private Location() { }
#pragma warning restore CS8618

    private Location(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Khởi tạo Location mới.
    /// </summary>
    public static Result<Location> Create(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
        {
            return Result.Failure<Location>(Error.Validation("Location.InvalidLatitude", "Vĩ độ (Latitude) phải nằm trong khoảng -90 đến 90."));
        }

        if (longitude < -180 || longitude > 180)
        {
            return Result.Failure<Location>(Error.Validation("Location.InvalidLongitude", "Kinh độ (Longitude) phải nằm trong khoảng -180 đến 180."));
        }

        return Result.Success(new Location(latitude, longitude));
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }
}
