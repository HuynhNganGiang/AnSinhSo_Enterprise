using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Notifications.DTOs;
using AnSinhSo.Application.Notifications.Services;
using MediatR;

namespace AnSinhSo.Application.Notifications.Queries.GetUnreadNotificationCount;

internal sealed class GetUnreadNotificationCountQueryHandler : IRequestHandler<GetUnreadNotificationCountQuery, UnreadCountDto>
{
    private readonly INotificationQueryService _queryService;

    public GetUnreadNotificationCountQueryHandler(INotificationQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<UnreadCountDto> Handle(GetUnreadNotificationCountQuery request, CancellationToken cancellationToken)
    {
        return await _queryService.GetUnreadCountAsync(request.CitizenId, cancellationToken);
    }
}
