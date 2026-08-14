using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.Queries.GetCurrentUserPermissions;
using AnSinhSo.Application.Authorization.Queries.GetCurrentUserProfile;
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

    public CurrentUserController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize] // Requires basic authentication
    [ProducesResponseType(typeof(AnSinhSo.Shared.Responses.ApiResult<CurrentUserProfileDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCurrentUserProfileQuery(), cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult<CurrentUserProfileDto>.SuccessResult(result.Value));
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
}
