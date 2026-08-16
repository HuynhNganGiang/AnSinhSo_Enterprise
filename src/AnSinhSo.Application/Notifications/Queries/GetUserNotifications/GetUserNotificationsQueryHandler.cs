using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Notifications.DTOs;
using AnSinhSo.Application.Notifications.Services;
using MediatR;

namespace AnSinhSo.Application.Notifications.Queries.GetUserNotifications;

internal sealed class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, List<NotificationDto>>
{
    private readonly INotificationQueryService _queryService;

    public GetUserNotificationsQueryHandler(INotificationQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<List<NotificationDto>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        return await _queryService.GetUserNotificationsAsync(request.CitizenId, cancellationToken);
    }
}
