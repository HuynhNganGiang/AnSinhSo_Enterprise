using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Caching;
using AnSinhSo.Application.Common.Models;
using AnSinhSo.Domain.Aggregates.RoleAggregate.Events;
using MediatR;

namespace AnSinhSo.Application.Authorization.EventHandlers;

public class RolePermissionChangedDomainEventHandler : INotificationHandler<DomainEventNotification<RolePermissionChangedDomainEvent>>
{
    private readonly IAuthorizationCacheService _cacheService;

    public RolePermissionChangedDomainEventHandler(IAuthorizationCacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Handle(DomainEventNotification<RolePermissionChangedDomainEvent> notification, CancellationToken cancellationToken)
    {
        await _cacheService.InvalidateRoleAsync(notification.DomainEvent.RoleId, cancellationToken);
    }
}
