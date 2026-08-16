using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Notifications.DTOs;
using AnSinhSo.Application.Notifications.Services;
using MediatR;

namespace AnSinhSo.Application.Notifications.Queries.GetNotifications;

internal sealed class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, List<NotificationDto>>
{
    private readonly INotificationQueryService _queryService;

    public GetNotificationsQueryHandler(INotificationQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<List<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        return await _queryService.GetNotificationsAsync(request, cancellationToken);
    }
}
