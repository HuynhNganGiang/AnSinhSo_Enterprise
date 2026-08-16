using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Map.DTOs;
using MediatR;

namespace AnSinhSo.Application.Map.Queries;

internal sealed class GetMapStatisticsQueryHandler : IRequestHandler<GetMapStatisticsQuery, MapStatisticsDto>
{
    private readonly IMapQueryService _mapQueryService;

    public GetMapStatisticsQueryHandler(IMapQueryService mapQueryService)
    {
        _mapQueryService = mapQueryService;
    }

    public async Task<MapStatisticsDto> Handle(GetMapStatisticsQuery request, CancellationToken cancellationToken)
    {
        return await _mapQueryService.GetMapStatisticsAsync(cancellationToken);
    }
}
