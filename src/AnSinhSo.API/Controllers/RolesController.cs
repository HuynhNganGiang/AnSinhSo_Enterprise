using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.Commands.CreateRole;
using AnSinhSo.Application.Authorization.Commands.DeleteRole;
using AnSinhSo.Application.Authorization.Commands.UpdateRole;
using AnSinhSo.Application.Authorization.Commands.UpdateRolePermissions;
using AnSinhSo.Application.Authorization.Queries.GetRoleDetail;
using AnSinhSo.Application.Authorization.Queries.GetRoles;
using AnSinhSo.Contracts.Authorization;
using AnSinhSo.Domain.Constants;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/roles")]
[Tags("Roles")] // AD #122
public sealed class RolesController : ApiControllerBase
{
    private readonly ISender _sender;

    public RolesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Policy = Permissions.Roles.View)]
    [ProducesResponseType(typeof(RoleDto[]), StatusCodes.Status200OK)] // Simplified for brevity, usually wrapped in Envelope/Pagination
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetRolesQuery(), cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        // According to AD #116, should return ApiResult envelope. We will assume ApiControllerBase handles it or we wrap it here.
        // Actually, the user asked for ApiResult<T>. So:
        return Ok(AnSinhSo.Shared.Responses.ApiResult<System.Collections.Generic.IReadOnlyCollection<AnSinhSo.Application.Authorization.DTOs.RoleDto>>.SuccessResult(result.Value));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = Permissions.Roles.View)]
    public async Task<IActionResult> GetRoleById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetRoleDetailQuery(id), cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult<AnSinhSo.Application.Authorization.DTOs.RoleDetailDto>.SuccessResult(result.Value));
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Roles.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateRoleCommand(request.Name, request.Description, false);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(nameof(GetRoleById), new { id = result.Value }, AnSinhSo.Shared.Responses.ApiResult<Guid>.SuccessResult(result.Value));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = Permissions.Roles.Update)]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateRoleCommand(id, request.Name, request.Description);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult.SuccessResult("Role updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = Permissions.Roles.Delete)]
    public async Task<IActionResult> DeleteRole(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteRoleCommand(id);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent(); // Idempotent DELETE returns 204
    }

    [HttpPut("{id:guid}/permissions")]
    [Authorize(Policy = Permissions.PermissionsModule.Manage)]
    public async Task<IActionResult> UpdateRolePermissions(Guid id, [FromBody] UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateRolePermissionsCommand(id, request.PermissionIds);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult.SuccessResult("Permissions synchronized successfully."));
    }
}
