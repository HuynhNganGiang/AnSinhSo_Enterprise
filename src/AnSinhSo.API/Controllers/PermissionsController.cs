using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.Queries.GetPermissions;
using AnSinhSo.Contracts.Authorization;
using AnSinhSo.Domain.Constants;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/permissions")]
[Tags("Permissions")]
public sealed class PermissionsController : ApiControllerBase
{
    private readonly ISender _sender;

    public PermissionsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Policy = Permissions.PermissionsModule.View)]
    public async Task<IActionResult> GetPermissions(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPermissionsQuery(), cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult<System.Collections.Generic.IReadOnlyCollection<AnSinhSo.Application.Authorization.DTOs.PermissionDto>>.SuccessResult(result.Value));
    }
}
