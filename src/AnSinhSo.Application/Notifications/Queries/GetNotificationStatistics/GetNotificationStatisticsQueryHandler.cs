using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Notifications.DTOs;
using AnSinhSo.Application.Notifications.Services;
using MediatR;

namespace AnSinhSo.Application.Notifications.Queries.GetNotificationStatistics;

internal sealed class GetNotificationStatisticsQueryHandler : IRequestHandler<GetNotificationStatisticsQuery, NotificationStatisticsDto>
{
    private readonly INotificationQueryService _queryService;

    public GetNotificationStatisticsQueryHandler(INotificationQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<NotificationStatisticsDto> Handle(GetNotificationStatisticsQuery request, CancellationToken cancellationToken)
    {
        return await _queryService.GetStatisticsAsync(cancellationToken);
    }
}
