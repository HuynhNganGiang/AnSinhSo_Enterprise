using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Caching;
using AnSinhSo.Application.Common.Models;
using AnSinhSo.Domain.Aggregates.UserRoleAggregate.Events;
using MediatR;

namespace AnSinhSo.Application.Authorization.EventHandlers;

public class UserRoleAssignedDomainEventHandler : INotificationHandler<DomainEventNotification<UserRoleAssignedDomainEvent>>
{
    private readonly IAuthorizationCacheService _cacheService;

    public UserRoleAssignedDomainEventHandler(IAuthorizationCacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Handle(DomainEventNotification<UserRoleAssignedDomainEvent> notification, CancellationToken cancellationToken)
    {
        await _cacheService.InvalidateCitizenAsync(notification.DomainEvent.CitizenIdentityId, cancellationToken);
    }
}
