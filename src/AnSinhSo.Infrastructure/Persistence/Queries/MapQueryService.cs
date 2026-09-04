using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Map.DTOs;
using AnSinhSo.Application.Map.Queries;
using AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate.Enumerations;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Queries;

internal sealed class MapQueryService : IMapQueryService
{
    private readonly AnSinhSoDbContext _context;

    public MapQueryService(AnSinhSoDbContext context)
    {
        _context = context;
    }

    public async Task<List<MapMarkerDto>> GetMapMarkersAsync(GetMapMarkersQuery query, CancellationToken cancellationToken = default)
    {
        var markers = new List<MapMarkerDto>();

        var kw = query.Keyword?.Trim().ToLower();

        if (query.Households)
        {
            /*
             * Household map rule
             * ------------------
             *
             * 1. If Location exists:
             *      use the stored coordinate.
             *
             * 2. If Location is missing:
             *      create a deterministic estimated coordinate
             *      from Address + HouseholdCode.
             *
             * The estimated location is NOT written to the database.
             * Therefore original production data remains intact.
             */

            var householdsQuery =
                _context.Households
                    .AsNoTracking()
                    .Where(
                        h =>
                            h.Status ==
                            HouseholdStatus.Active);

            if (!string.IsNullOrEmpty(kw))
            {
                householdsQuery =
                    householdsQuery.Where(
                        h =>
                            h.HouseholdCode.Value
                                .ToLower()
                                .Contains(kw));
            }

            var householdEntities =
                await householdsQuery
                    .ToListAsync(
                        cancellationToken);

            if (query.Poor == true)
            {
                householdEntities =
                    householdEntities
                        .Where(
                            h =>
                                h.CurrentClassification.Id ==
                                HouseholdClassification.Poor.Id)
                        .ToList();
            }
            else if (query.NearPoor == true)
            {
                householdEntities =
                    householdEntities
                        .Where(
                            h =>
                                h.CurrentClassification.Id ==
                                HouseholdClassification.NearPoor.Id)
                        .ToList();
            }

            const double centerLatitude =
                11.21011269694565;

            const double centerLongitude =
                108.32172004484949;

            var households =
                householdEntities
                    .Select(
                        h =>
                        {
                            var estimated =
                                h.Location == null;

                            double latitude;
                            double longitude;

                            if (!estimated)
                            {
                                latitude =
                                    h.Location!.Latitude;

                                longitude =
                                    h.Location.Longitude;
                            }
                            else
                            {
                                /*
                                 * Same household/address always receives
                                 * the same estimated location.
                                 */

                                var key =
                                    string.Join(
                                        "|",
                                        h.HouseholdCode.Value,
                                        h.Address.Street,
                                        h.Address.Ward,
                                        h.Address.District,
                                        h.Address.Province);

                                var hash =
                                    System.Security.Cryptography
                                        .SHA256
                                        .HashData(
                                            System.Text.Encoding.UTF8
                                                .GetBytes(key));

                                var a =
                                    System.BitConverter
                                        .ToUInt32(hash, 0)
                                    /
                                    (double)uint.MaxValue;

                                var b =
                                    System.BitConverter
                                        .ToUInt32(hash, 4)
                                    /
                                    (double)uint.MaxValue;

                                /*
                                 * 350 m - approximately 4 km
                                 * around the reference point.
                                 *
                                 * sqrt gives more natural spatial
                                 * distribution instead of crowding
                                 * the center.
                                 */

                                var radiusKm =
                                    0.35 +
                                    3.65 *
                                    System.Math.Sqrt(b);

                                var angle =
                                    2.0 *
                                    System.Math.PI *
                                    a;

                                var latitudeOffset =
                                    radiusKm *
                                    System.Math.Cos(angle)
                                    /
                                    111.32;

                                var longitudeOffset =
                                    radiusKm *
                                    System.Math.Sin(angle)
                                    /
                                    (
                                        111.32 *
                                        System.Math.Cos(
                                            centerLatitude *
                                            System.Math.PI /
                                            180.0)
                                    );

                                latitude =
                                    centerLatitude +
                                    latitudeOffset;

                                longitude =
                                    centerLongitude +
                                    longitudeOffset;
                            }

                            var color =
                                h.CurrentClassification.Id ==
                                HouseholdClassification.Poor.Id
                                    ? "red"
                                    : h.CurrentClassification.Id ==
                                      HouseholdClassification.NearPoor.Id
                                        ? "yellow"
                                        : "green";

                            var locationNote =
                                estimated
                                    ? "\nVị trí: Ước tính theo địa chỉ"
                                    : "\nVị trí: Tọa độ đã lưu";

                            return new MapMarkerDto
                            {
                                Id = h.Id.Value,

                                MarkerType =
                                    "Household",

                                Name =
                                    $"Hộ gia đình {h.HouseholdCode.Value}",

                                Latitude =
                                    latitude,

                                Longitude =
                                    longitude,

                                Color =
                                    color,

                                PopupTitle =
                                    $"Hộ gia đình: {h.HouseholdCode.Value}",

                                PopupContent =
                                    $"Địa chỉ: {h.Address.Street}, " +
                                    $"{h.Address.Ward}, " +
                                    $"{h.Address.District}, " +
                                    $"{h.Address.Province}" +
                                    locationNote,

                                Status =
                                    h.Status.ToString()
                            };
                        })
                    .ToList();

            markers.AddRange(
                households);
        }
        if (query.Citizens)
        {
            var citizensQuery = _context.Citizens
                .Where(c => c.Location != null && c.Status == CitizenStatus.Active);

            if (!string.IsNullOrEmpty(kw))
            {
                citizensQuery = citizensQuery.Where(c => 
                    c.FullName.FirstName.ToLower().Contains(kw) || 
                    c.FullName.LastName.ToLower().Contains(kw) || 
                    (c.CitizenNumber != null && c.CitizenNumber.Value.ToLower().Contains(kw)));
            }

            var citizens = await citizensQuery
                .Select(c => new MapMarkerDto
                {
                    Id = c.Id.Value,
                    MarkerType = "Citizen",
                    Name = c.FullName.LastName + " " + c.FullName.FirstName,
                    Latitude = c.Location!.Latitude,
                    Longitude = c.Location.Longitude,
                    Color = "green",
                    PopupTitle = $"Công dân: {c.FullName.LastName} {c.FullName.FirstName}",
                    PopupContent = $"CCCD: {(c.CitizenNumber == null ? string.Empty : c.CitizenNumber.Value)}\nSĐT: {(c.PhoneNumber == null ? string.Empty : c.PhoneNumber.Value)}",
                    Status = c.Status.ToString()
                })
                .ToListAsync(cancellationToken);

            markers.AddRange(citizens);
        }

        if (query.PaymentPoints)
        {
            var ppQuery = _context.PaymentPoints
                .Where(p => p.Location != null && p.Status == Domain.Aggregates.PaymentPointAggregate.PaymentPointStatus.Active);

            if (!string.IsNullOrEmpty(kw))
            {
                ppQuery = ppQuery.Where(p => p.Name.ToLower().Contains(kw) || p.Code.ToLower().Contains(kw));
            }

            var points = await ppQuery
                .Select(p => new MapMarkerDto
                {
                    Id = p.Id.Value,
                    MarkerType = "PaymentPoint",
                    Name = p.Name,
                    Latitude = p.Location!.Latitude,
                    Longitude = p.Location.Longitude,
                    Color = "blue",
                    PopupTitle = p.Name,
                    PopupContent = $"Địa chỉ: {p.Address.Street}, {p.Address.Ward}\nMô tả: {p.Description}",
                    Status = p.Status.ToString()
                })
                .ToListAsync(cancellationToken);

            markers.AddRange(points);
        }

        if (query.Welfare)
        {
            // Welfare cases map to the citizen's location
            var welfareQuery = from w in _context.WelfareCases
                               join c in _context.Citizens on w.CitizenId equals c.Id
                               where c.Location != null
                               select new { w, c };

            var cases = await welfareQuery
                .Select(x => new MapMarkerDto
                {
                    Id = x.w.Id.Value,
                    MarkerType = "Welfare",
                    Name = x.c.FullName.LastName + " " + x.c.FullName.FirstName,
                    Latitude = x.c.Location!.Latitude,
                    Longitude = x.c.Location.Longitude,
                    Color = "purple",
                    PopupTitle = $"Trợ cấp: {x.c.FullName.LastName} {x.c.FullName.FirstName}",
                    PopupContent = $"Chương trình: {x.w.ProgramId.Value}",
                    Status = "Welfare"
                })
                .ToListAsync(cancellationToken);

            markers.AddRange(cases);
        }

        return markers;
    }

    public async Task<MapStatisticsDto> GetMapStatisticsAsync(CancellationToken cancellationToken = default)
    {
        var citizenCount = await _context.Citizens.CountAsync(c => c.Location != null, cancellationToken);
        var householdCount = await _context.Households.CountAsync(h => h.Location != null, cancellationToken);
        
        // Mock counts for poor/near poor for now as it requires complex domain logic
        var poorCount = 0; 
        var nearPoorCount = 0;

        var paymentPointCount = await _context.PaymentPoints.CountAsync(p => p.Location != null, cancellationToken);
        var welfareCount = await _context.WelfareCases.CountAsync(cancellationToken);

        return new MapStatisticsDto
        {
            CitizenCount = citizenCount,
            HouseholdCount = householdCount,
            PoorCount = poorCount,
            NearPoorCount = nearPoorCount,
            PaymentPointCount = paymentPointCount,
            WelfareCount = welfareCount
        };
    }
}
