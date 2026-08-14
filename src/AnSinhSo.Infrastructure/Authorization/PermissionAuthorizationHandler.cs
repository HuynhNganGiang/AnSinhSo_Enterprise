using System;
using System.Linq;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.Abstractions;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AnSinhSo.Infrastructure.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Identity is null || !context.User.Identity.IsAuthenticated)
        {
            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();
        var currentUser = scope.ServiceProvider.GetRequiredService<ICurrentUser>();
        
        if (string.IsNullOrEmpty(currentUser.UserId) || !Guid.TryParse(currentUser.UserId, out var userIdGuid))
        {
            return;
        }

        var citizenIdentityId = CitizenIdentityId.Create(userIdGuid);
        var permissionResolver = scope.ServiceProvider.GetRequiredService<IPermissionResolver>();

        var permissions = await permissionResolver.GetPermissionsAsync(citizenIdentityId);

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}
