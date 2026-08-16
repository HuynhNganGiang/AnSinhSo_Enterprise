using System;
using System.Threading.Tasks;
using AnSinhSo.Application.Households.Commands.ActivateHousehold;
using AnSinhSo.Application.Households.Commands.AddHouseholdMember;
using AnSinhSo.Application.Households.Commands.ChangeHouseholdHead;
using AnSinhSo.Application.Households.Commands.CreateHousehold;
using AnSinhSo.Application.Households.Commands.DeactivateHousehold;
using AnSinhSo.Application.Households.Commands.RemoveHouseholdMember;
using AnSinhSo.Application.Households.Commands.UpdateHouseholdAddress;
using AnSinhSo.Application.Households.Queries.GetHouseholdById;
using AnSinhSo.Application.Households.Queries.SearchHouseholds;
using AnSinhSo.Application.Households.DTOs;
using AnSinhSo.Contracts.Common;
using AnSinhSo.Domain.Constants;
using AnSinhSo.Shared.Responses;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/households")]
[ApiController]
[Authorize]
[Tags("Households")]
public class HouseholdsController : ApiControllerBase
{
    private readonly ISender _sender;

    public HouseholdsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Policy = Permissions.Households.Read)]
    [ProducesResponseType(typeof(ApiResult<PagedResult<HouseholdSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] SearchHouseholdsQuery query)
    {
        var result = await _sender.Send(query);
        return result.IsSuccess ? Ok(ApiResult<PagedResult<HouseholdSummaryDto>>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = Permissions.Households.Read)]
    [ProducesResponseType(typeof(ApiResult<HouseholdDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetHouseholdByIdQuery(id);
        var result = await _sender.Send(query);
        return result.IsSuccess ? Ok(ApiResult<HouseholdDto>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Households.Create)]
    [ProducesResponseType(typeof(ApiResult<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateHouseholdCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsFailure) return HandleFailure(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, ApiResult<Guid>.SuccessResult(result.Value));
    }

    [HttpPatch("{id:guid}/activate")]
    [Authorize(Policy = Permissions.Households.Update)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid id)
    {
        var command = new ActivateHouseholdCommand(id);
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(ApiResult<bool>.SuccessResult(true)) : HandleFailure(result);
    }

    [HttpPatch("{id:guid}/deactivate")]
    [Authorize(Policy = Permissions.Households.Update)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var command = new DeactivateHouseholdCommand(id);
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(ApiResult<bool>.SuccessResult(true)) : HandleFailure(result);
    }

    [HttpPut("{id:guid}/address")]
    [Authorize(Policy = Permissions.Households.Update)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAddress(Guid id, [FromBody] UpdateHouseholdAddressCommand command)
    {
        var request = command with { HouseholdId = id };
        var result = await _sender.Send(request);
        return result.IsSuccess ? Ok(ApiResult<bool>.SuccessResult(true)) : HandleFailure(result);
    }

    [HttpPost("{id:guid}/members")]
    [Authorize(Policy = Permissions.Households.Update)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddMember(Guid id, [FromBody] AddHouseholdMemberCommand command)
    {
        var request = command with { HouseholdId = id };
        var result = await _sender.Send(request);
        return result.IsSuccess ? Ok(ApiResult<bool>.SuccessResult(true)) : HandleFailure(result);
    }

    [HttpDelete("{id:guid}/members/{memberId:guid}")]
    [Authorize(Policy = Permissions.Households.Update)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveMember(Guid id, Guid memberId)
    {
        var command = new RemoveHouseholdMemberCommand(id, memberId);
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(ApiResult<bool>.SuccessResult(true)) : HandleFailure(result);
    }

    [HttpPut("{id:guid}/head")]
    [Authorize(Policy = Permissions.Households.Update)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeHead(Guid id, [FromBody] ChangeHouseholdHeadCommand command)
    {
        var request = command with { HouseholdId = id };
        var result = await _sender.Send(request);
        return result.IsSuccess ? Ok(ApiResult<bool>.SuccessResult(true)) : HandleFailure(result);
    }

    [HttpPatch("{id:guid}/location")]
    [Authorize(Policy = Permissions.Map.UpdateLocation)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLocation(Guid id, [FromBody] UpdateLocationRequest request)
    {
        var command = new AnSinhSo.Application.Households.Commands.UpdateLocation.UpdateHouseholdLocationCommand(id, request.Latitude, request.Longitude);
        var result = await _sender.Send(command);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(ApiResult<bool>.SuccessResult(true));
    }
}
