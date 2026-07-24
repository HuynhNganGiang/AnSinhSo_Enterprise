using System;
using System.Threading.Tasks;
using AnSinhSo.Application.Households.Commands.ActivateHousehold;
using AnSinhSo.Application.Households.Commands.AddHouseholdMember;
using AnSinhSo.Application.Households.Commands.ChangeHouseholdHead;
using AnSinhSo.Application.Households.Commands.CreateHousehold;
using AnSinhSo.Application.Households.Commands.DeactivateHousehold;
using AnSinhSo.Application.Households.Commands.RemoveHouseholdMember;
using AnSinhSo.Application.Households.Queries.GetHouseholdById;
using AnSinhSo.Application.Households.Queries.GetHouseholdList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/households")]
public class HouseholdsController : ControllerBase
{
    private readonly IMediator _mediator;

    public HouseholdsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] GetHouseholdListQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetHouseholdByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHouseholdCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var command = new ActivateHouseholdCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var command = new DeactivateHouseholdCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/members")]
    public async Task<IActionResult> AddMember(Guid id, [FromBody] AddHouseholdMemberCommand command)
    {
        var request = command with { HouseholdId = id };
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpDelete("{id}/members/{memberId}")]
    public async Task<IActionResult> RemoveMember(Guid id, Guid memberId)
    {
        var command = new RemoveHouseholdMemberCommand(id, memberId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}/head")]
    public async Task<IActionResult> ChangeHead(Guid id, [FromBody] ChangeHouseholdHeadCommand command)
    {
        var request = command with { HouseholdId = id };
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}
