using System;
using System.Threading.Tasks;
using AnSinhSo.Application.Citizens.Commands.ActivateCitizen;
using AnSinhSo.Application.Citizens.Commands.CreateCitizen;
using AnSinhSo.Application.Citizens.Commands.DeactivateCitizen;
using AnSinhSo.Application.Citizens.Commands.UpdateCitizen;
using AnSinhSo.Application.Citizens.Queries.GetCitizenById;
using AnSinhSo.Application.Citizens.Queries.GetCitizens;
using AnSinhSo.Application.Citizens.Queries.SearchCitizen;
using AnSinhSo.Application.Citizens.Queries.SearchCitizenByIdentityNumber;
using AnSinhSo.Contracts.Citizens;
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
[Route("api/v{version:apiVersion}/citizens")]
[ApiController]
[Authorize]
public class CitizensController : ApiControllerBase
{
    private readonly ISender _sender;

    public CitizensController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Policy = Permissions.Citizens.View)]
    [ProducesResponseType(typeof(ApiResult<PagedResult<CitizenDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCitizens([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sort = null)
    {
        var query = new GetCitizensQuery(page, pageSize, sort);
        var result = await _sender.Send(query);
        return result.IsSuccess ? Ok(ApiResult<PagedResult<CitizenDto>>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    [HttpGet("search")]
    [Authorize(Policy = Permissions.Citizens.View)]
    [ProducesResponseType(typeof(ApiResult<PagedResult<CitizenDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] SearchCitizenRequest request)
    {
        var query = new SearchCitizenQuery(request.IdentityNumber, request.Phone, request.Keyword, request.Page, request.PageSize, request.Sort);
        var result = await _sender.Send(query);
        return result.IsSuccess ? Ok(ApiResult<PagedResult<CitizenDto>>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = Permissions.Citizens.View)]
    [ProducesResponseType(typeof(ApiResult<CitizenDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetCitizenByIdQuery(id);
        var result = await _sender.Send(query);
        return result.IsSuccess ? Ok(ApiResult<CitizenDetailDto>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    [HttpGet("identity/{identityNumber}")]
    [Authorize(Policy = Permissions.Citizens.View)]
    [ProducesResponseType(typeof(ApiResult<CitizenDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdentityNumber(string identityNumber)
    {
        var query = new SearchCitizenByIdentityNumberQuery(identityNumber);
        var result = await _sender.Send(query);
        return result.IsSuccess ? Ok(ApiResult<CitizenDetailDto>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Citizens.Create)]
    [ProducesResponseType(typeof(ApiResult<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCitizenRequest request)
    {
        var command = new CreateCitizenCommand(request.CitizenNumber, request.FullName, request.BirthDate, request.Gender, request.PhoneNumber, request.Address, request.Email);
        var result = await _sender.Send(command);

        if (result.IsFailure) return HandleFailure(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, ApiResult<Guid>.SuccessResult(result.Value));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = Permissions.Citizens.Update)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCitizenRequest request)
    {
        var command = new UpdateCitizenCommand(id, request.FullName, request.BirthDate, request.Gender, request.PhoneNumber, request.Address, request.Email);
        var result = await _sender.Send(command);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(ApiResult<bool>.SuccessResult(true));
    }

    [HttpPatch("{id:guid}/activate")]
    [Authorize(Policy = Permissions.Citizens.Activate)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid id)
    {
        var command = new ActivateCitizenCommand(id);
        var result = await _sender.Send(command);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(ApiResult<bool>.SuccessResult(true));
    }

    [HttpPatch("{id:guid}/deactivate")]
    [Authorize(Policy = Permissions.Citizens.Deactivate)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var command = new DeactivateCitizenCommand(id);
        var result = await _sender.Send(command);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(ApiResult<bool>.SuccessResult(true));
    }
}
