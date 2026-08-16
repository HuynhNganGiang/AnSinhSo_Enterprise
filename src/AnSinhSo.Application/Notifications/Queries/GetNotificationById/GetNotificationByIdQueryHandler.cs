using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Notifications.DTOs;
using AnSinhSo.Application.Notifications.Services;
using MediatR;

namespace AnSinhSo.Application.Notifications.Queries.GetNotificationById;

internal sealed class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, NotificationDto>
{
    private readonly INotificationQueryService _queryService;

    public GetNotificationByIdQueryHandler(INotificationQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<NotificationDto> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
    {
        return await _queryService.GetNotificationByIdAsync(request.Id, cancellationToken);
    }
}
