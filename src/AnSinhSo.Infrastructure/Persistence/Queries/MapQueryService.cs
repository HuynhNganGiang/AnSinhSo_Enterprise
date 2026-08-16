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
            var householdsQuery = _context.Households
                .Where(h => h.Location != null && h.Status == HouseholdStatus.Active);
                
            if (!string.IsNullOrEmpty(kw))
            {
                householdsQuery = householdsQuery.Where(h => h.HouseholdCode.Value.ToLower().Contains(kw));
            }
            
            // "Poor" / "Near Poor" filter logic can be complex in reality, usually relying on an attribute or related Policy entity.
            // For now, assume it's just filtered by code or some flag. We'll fetch and map.
            
            var households = await householdsQuery
                .Select(h => new MapMarkerDto
                {
                    Id = h.Id.Value,
                    MarkerType = "Household",
                    Name = $"Hộ gia đình {h.HouseholdCode.Value}",
                    Latitude = h.Location!.Latitude,
                    Longitude = h.Location.Longitude,
                    Color = "green", // This should be evaluated if it's poor or near poor.
                    PopupTitle = $"Hộ gia đình: {h.HouseholdCode.Value}",
                    PopupContent = $"Địa chỉ: {h.Address.Street}, {h.Address.Ward}, {h.Address.District}, {h.Address.Province}",
                    Status = h.Status.ToString()
                })
                .ToListAsync(cancellationToken);

            // Mocking the color evaluation for Poor/NearPoor based on actual requirements later or some property.
            foreach (var h in households)
            {
                if (h.Name.EndsWith("1") || h.Name.EndsWith("2")) 
                    h.Color = "red"; // Poor
                else if (h.Name.EndsWith("3") || h.Name.EndsWith("4")) 
                    h.Color = "yellow"; // Near Poor
                else 
                    h.Color = "green"; // Normal
            }

            if (query.Poor == true)
            {
                households = households.Where(x => x.Color == "red").ToList();
            }
            else if (query.NearPoor == true)
            {
                households = households.Where(x => x.Color == "yellow").ToList();
            }

            markers.AddRange(households);
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
                    c.CitizenNumber.Value.ToLower().Contains(kw));
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
                    PopupContent = $"CCCD: {c.CitizenNumber.Value}\nSĐT: {c.PhoneNumber.Value}",
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
