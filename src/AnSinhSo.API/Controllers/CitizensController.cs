using System;
using System.Threading.Tasks;
using AnSinhSo.Application.Citizens.Commands.ActivateCitizen;
using AnSinhSo.Application.Citizens.Commands.ChangeCitizenAddress;
using AnSinhSo.Application.Citizens.Commands.ChangeCitizenPhone;
using AnSinhSo.Application.Citizens.Commands.CreateCitizen;
using AnSinhSo.Application.Citizens.Commands.DeactivateCitizen;
using AnSinhSo.Application.Citizens.Queries.GetCitizenById;
using AnSinhSo.Application.Citizens.Queries.GetCitizenList;
using AnSinhSo.Application.Citizens.Queries.SearchCitizen;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/citizens")]
public class CitizensController : ControllerBase
{
    private readonly IMediator _mediator;

    public CitizensController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] GetCitizenListQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetCitizenByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] SearchCitizenQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCitizenCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}/address")]
    public async Task<IActionResult> ChangeAddress(Guid id, [FromBody] ChangeCitizenAddressCommand command)
    {
        var request = command with { CitizenId = id };
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPut("{id}/phone")]
    public async Task<IActionResult> ChangePhone(Guid id, [FromBody] ChangeCitizenPhoneCommand command)
    {
        var request = command with { CitizenId = id };
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("{id}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var command = new ActivateCitizenCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var command = new DeactivateCitizenCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
