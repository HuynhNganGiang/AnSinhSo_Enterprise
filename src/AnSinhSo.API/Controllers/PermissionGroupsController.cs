using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.Queries.GetPermissionGroups;
using AnSinhSo.Contracts.Authorization;
using AnSinhSo.Domain.Constants;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/permission-groups")]
[Tags("Permission Groups")]
public sealed class PermissionGroupsController : ApiControllerBase
{
    private readonly ISender _sender;

    public PermissionGroupsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Policy = Permissions.PermissionsModule.View)]
    public async Task<IActionResult> GetPermissionGroups(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPermissionGroupsQuery(), cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult<System.Collections.Generic.IReadOnlyCollection<AnSinhSo.Application.Authorization.DTOs.PermissionGroupWithPermissionsDto>>.SuccessResult(result.Value));
    }
}
