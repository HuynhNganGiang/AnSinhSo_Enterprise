using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AnSinhSo.Shared.Responses;
using Asp.Versioning;
using MediatR;
using AnSinhSo.Application.WelfareCases.Commands.CancelWelfareCase;
using AnSinhSo.Application.WelfareCases.Commands.CloseWelfareCase;
using AnSinhSo.Application.WelfareCases.Commands.CreateWelfareCase;
using AnSinhSo.Application.WelfareCases.Commands.MakeWelfareCaseDecision;
using AnSinhSo.Application.WelfareCases.Commands.SubmitWelfareCase;
using AnSinhSo.Application.WelfareCases.Commands.UpdateWelfareCase;
using AnSinhSo.Application.WelfareCases.DTOs;
using AnSinhSo.Application.WelfareCases.Queries.GetCitizenWelfareCases;
using AnSinhSo.Application.WelfareCases.Queries.GetWelfareCaseById;
using AnSinhSo.Application.WelfareCases.Queries.SearchWelfareCases;
using AnSinhSo.Contracts.Common;
using AnSinhSo.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/welfarecases")]
[ApiController]
public sealed class WelfareCasesController : ApiControllerBase
{
    private readonly ISender _sender;

    public WelfareCasesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Authorize(Policy = Permissions.WelfareCases.Create)]
    [ProducesResponseType(typeof(ApiResult<Guid>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateWelfareCase([FromBody] CreateWelfareCaseCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetWelfareCaseById), new { id = result.Value }, ApiResult<Guid>.SuccessResult(result.Value));
        }
        return HandleFailure(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = Permissions.WelfareCases.View)]
    [ProducesResponseType(typeof(ApiResult<WelfareCaseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWelfareCaseById(Guid id)
    {
        var result = await _sender.Send(new GetWelfareCaseByIdQuery(id));
        return result.IsSuccess ? Ok(ApiResult<WelfareCaseDto>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    [HttpGet]
    [Authorize(Policy = Permissions.WelfareCases.View)]
    [ProducesResponseType(typeof(ApiResult<PagedResult<WelfareCaseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchWelfareCases(
        [FromQuery] string? keyword,
        [FromQuery] Guid? programId,
        [FromQuery] int? statusId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sort = null)
    {
        var query = new SearchWelfareCasesQuery(keyword, programId, statusId, page, pageSize, sort);
        var result = await _sender.Send(query);
        return result.IsSuccess ? Ok(ApiResult<PagedResult<WelfareCaseDto>>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    [HttpGet("citizen/{citizenId:guid}")]
    [Authorize(Policy = Permissions.WelfareCases.View)]
    [ProducesResponseType(typeof(ApiResult<IReadOnlyList<WelfareCaseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCitizenWelfareCases(Guid citizenId)
    {
        var result = await _sender.Send(new GetCitizenWelfareCasesQuery(citizenId));
        return result.IsSuccess ? Ok(ApiResult<IReadOnlyList<WelfareCaseDto>>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = Permissions.WelfareCases.Update)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateWelfareCase(Guid id, [FromBody] UpdateWelfareCaseCommand command)
    {
        if (id != command.WelfareCaseId) return BadRequest();
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(ApiResult.SuccessResult()) : HandleFailure(result);
    }

    [HttpPost("{id:guid}/submit")]
    [Authorize(Policy = Permissions.WelfareCases.Update)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> SubmitWelfareCase(Guid id, [FromBody] SubmitWelfareCaseCommand command)
    {
        if (id != command.WelfareCaseId) return BadRequest();
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(ApiResult.SuccessResult()) : HandleFailure(result);
    }

    [HttpPatch("{id:guid}/decision")]
    [Authorize(Policy = Permissions.WelfareCases.Decide)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> MakeWelfareCaseDecision(Guid id, [FromBody] MakeWelfareCaseDecisionCommand command)
    {
        if (id != command.WelfareCaseId) return BadRequest();
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(ApiResult.SuccessResult()) : HandleFailure(result);
    }

    [HttpPost("{id:guid}/cancel")]
    [Authorize(Policy = Permissions.WelfareCases.Cancel)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CancelWelfareCase(Guid id, [FromBody] CancelWelfareCaseCommand command)
    {
        if (id != command.WelfareCaseId) return BadRequest();
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(ApiResult.SuccessResult()) : HandleFailure(result);
    }

    [HttpPost("{id:guid}/close")]
    [Authorize(Policy = Permissions.WelfareCases.Close)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CloseWelfareCase(Guid id, [FromBody] CloseWelfareCaseCommand command)
    {
        if (id != command.WelfareCaseId) return BadRequest();
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(ApiResult.SuccessResult()) : HandleFailure(result);
    }
}
