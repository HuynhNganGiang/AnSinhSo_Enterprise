using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Map.DTOs;
using MediatR;

namespace AnSinhSo.Application.Map.Queries;

internal sealed class GetMapMarkersQueryHandler : IRequestHandler<GetMapMarkersQuery, List<MapMarkerDto>>
{
    private readonly IMapQueryService _mapQueryService;

    public GetMapMarkersQueryHandler(IMapQueryService mapQueryService)
    {
        _mapQueryService = mapQueryService;
    }

    public async Task<List<MapMarkerDto>> Handle(GetMapMarkersQuery request, CancellationToken cancellationToken)
    {
        return await _mapQueryService.GetMapMarkersAsync(request, cancellationToken);
    }
}
