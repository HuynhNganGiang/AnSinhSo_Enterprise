using System.Threading.Tasks;
using AnSinhSo.Application.WelfarePrograms.Queries.GetWelfarePrograms;
using AnSinhSo.Application.WelfarePrograms.DTOs;
using AnSinhSo.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AnSinhSo.Shared.Responses;
using Asp.Versioning;
using MediatR;

namespace AnSinhSo.API.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/welfareprograms")]
[ApiController]
public sealed class WelfareProgramsController : ApiControllerBase
{
    private readonly ISender _sender;

    public WelfareProgramsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Policy = Permissions.WelfarePrograms.View)]
    [ProducesResponseType(typeof(ApiResult<IReadOnlyList<WelfareProgramDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWelfarePrograms()
    {
        var result = await _sender.Send(new GetWelfareProgramsQuery());
        return result.IsSuccess ? Ok(ApiResult<IReadOnlyList<WelfareProgramDto>>.SuccessResult(result.Value)) : HandleFailure(result);
    }
}
