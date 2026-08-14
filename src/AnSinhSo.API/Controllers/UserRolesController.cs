using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.Commands.AssignRole;
using AnSinhSo.Application.Authorization.Commands.RevokeRole;
using AnSinhSo.Application.Authorization.Queries.GetUserRoles;
using AnSinhSo.Contracts.Authorization;
using AnSinhSo.Domain.Constants;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/users")]
[Tags("User Roles")]
public sealed class UserRolesController : ApiControllerBase
{
    private readonly ISender _sender;

    public UserRolesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id:guid}/roles")]
    [Authorize(Policy = Permissions.UserRoles.View)]
    public async Task<IActionResult> GetUserRoles(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetUserRolesQuery(id), cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult<System.Collections.Generic.IReadOnlyCollection<AnSinhSo.Application.Authorization.DTOs.RoleDto>>.SuccessResult(result.Value));
    }

    [HttpPost("{id:guid}/roles")]
    [Authorize(Policy = Permissions.UserRoles.Manage)]
    public async Task<IActionResult> AssignRole(Guid id, [FromBody] AssignRoleRequest request, CancellationToken cancellationToken)
    {
        var command = new AssignRoleCommand(id, request.RoleId);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult.SuccessResult("Role assigned successfully."));
    }

    [HttpDelete("{id:guid}/roles/{roleId:guid}")]
    [Authorize(Policy = Permissions.UserRoles.Manage)]
    public async Task<IActionResult> RevokeRole(Guid id, Guid roleId, CancellationToken cancellationToken)
    {
        var command = new RevokeRoleCommand(id, roleId);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent(); // Idempotent DELETE
    }
}
