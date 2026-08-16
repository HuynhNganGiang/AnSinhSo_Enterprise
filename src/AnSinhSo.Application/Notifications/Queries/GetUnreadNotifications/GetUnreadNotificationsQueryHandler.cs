using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Notifications.DTOs;
using AnSinhSo.Application.Notifications.Services;
using MediatR;

namespace AnSinhSo.Application.Notifications.Queries.GetUnreadNotifications;

internal sealed class GetUnreadNotificationsQueryHandler : IRequestHandler<GetUnreadNotificationsQuery, List<NotificationDto>>
{
    private readonly INotificationQueryService _queryService;

    public GetUnreadNotificationsQueryHandler(INotificationQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<List<NotificationDto>> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken)
    {
        return await _queryService.GetUnreadNotificationsAsync(cancellationToken);
    }
}
