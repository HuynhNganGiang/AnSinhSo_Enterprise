using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Map.DTOs;

namespace AnSinhSo.Application.Map.Queries;

public interface IMapQueryService
{
    Task<List<MapMarkerDto>> GetMapMarkersAsync(GetMapMarkersQuery query, CancellationToken cancellationToken = default);
    Task<MapStatisticsDto> GetMapStatisticsAsync(CancellationToken cancellationToken = default);
}
