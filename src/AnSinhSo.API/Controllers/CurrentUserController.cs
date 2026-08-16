using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.Queries.GetCurrentUserPermissions;
using AnSinhSo.Application.Citizens.Queries.GetCitizenById;
using AnSinhSo.Contracts.Citizens;
using AnSinhSo.Application.Authorization.Queries.GetCurrentUserRoles;
using AnSinhSo.Contracts.Authorization;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/users/me")]
[Tags("Current User")]
public sealed class CurrentUserController : ApiControllerBase
{
    private readonly ISender _sender;
    private readonly AnSinhSo.Domain.Interfaces.ICurrentUser _currentUser;

    public CurrentUserController(ISender sender, AnSinhSo.Domain.Interfaces.ICurrentUser currentUser)
    {
        _sender = sender;
        _currentUser = currentUser;
    }

    [HttpGet("profile")]
    [Authorize] // Requires basic authentication
    [ProducesResponseType(typeof(AnSinhSo.Shared.Responses.ApiResult<CitizenDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        if (!System.Guid.TryParse(_currentUser.UserId, out var citizenIdentityId))
        {
            return Unauthorized();
        }

        // TODO: In a real scenario, we might need to map CitizenIdentityId to CitizenId
        // but since we are using existing GetCitizenByIdQuery, we assume CitizenId is passed or
        // we need another query GetCitizenByIdentityIdQuery. Wait, AD #100 says CitizenIdentity maps to Citizen.
        // Let's create a new query `GetCitizenByCitizenIdentityIdQuery`? No, the correction says:
        // "Correction: Use existing GetCitizenByIdQuery for CurrentUserController instead of creating a new query."
        // We will query the DB for the CitizenId if needed.
        
        // Wait, how to get CitizenId from ICurrentUser?
        var identityResult = await _sender.Send(new AnSinhSo.Application.Authorization.Queries.GetCurrentUserProfile.GetCurrentUserProfileQuery(), cancellationToken);
        if(identityResult.IsFailure) return HandleFailure(identityResult);

        var result = await _sender.Send(new GetCitizenByIdQuery(identityResult.Value.Id), cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult<CitizenDetailDto>.SuccessResult(result.Value));
    }

    [HttpGet("roles")]
    [Authorize]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCurrentUserRolesQuery(), cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult<System.Collections.Generic.List<RoleDto>>.SuccessResult(result.Value));
    }

    [HttpGet("permissions")]
    [Authorize]
    public async Task<IActionResult> GetPermissions(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCurrentUserPermissionsQuery(), cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult<System.Collections.Generic.List<string>>.SuccessResult(result.Value));
    }

    [HttpGet("notifications")]
    [Authorize]
    public async Task<IActionResult> GetNotifications(CancellationToken cancellationToken)
    {
        var identityResult = await _sender.Send(new AnSinhSo.Application.Authorization.Queries.GetCurrentUserProfile.GetCurrentUserProfileQuery(), cancellationToken);
        if (identityResult.IsFailure) return HandleFailure(identityResult);

        var result = await _sender.Send(new AnSinhSo.Application.Notifications.Queries.GetUserNotifications.GetUserNotificationsQuery(identityResult.Value.Id), cancellationToken);
        
        return Ok(AnSinhSo.Shared.Responses.ApiResult<System.Collections.Generic.List<AnSinhSo.Application.Notifications.DTOs.NotificationDto>>.SuccessResult(result));
    }

    [HttpGet("unread-count")]
    [Authorize]
    public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken)
    {
        var identityResult = await _sender.Send(new AnSinhSo.Application.Authorization.Queries.GetCurrentUserProfile.GetCurrentUserProfileQuery(), cancellationToken);
        if (identityResult.IsFailure) return HandleFailure(identityResult);

        var result = await _sender.Send(new AnSinhSo.Application.Notifications.Queries.GetUnreadNotificationCount.GetUnreadNotificationCountQuery(identityResult.Value.Id), cancellationToken);
        
        return Ok(AnSinhSo.Shared.Responses.ApiResult<AnSinhSo.Application.Notifications.DTOs.UnreadCountDto>.SuccessResult(result));
    }
}
