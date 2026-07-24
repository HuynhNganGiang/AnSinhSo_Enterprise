using System;
using System.Threading.Tasks;
using AnSinhSo.Application.WelfareGroups.Commands.ActivateWelfareGroup;
using AnSinhSo.Application.WelfareGroups.Commands.ChangeWelfareGroupName;
using AnSinhSo.Application.WelfareGroups.Commands.CreateWelfareGroup;
using AnSinhSo.Application.WelfareGroups.Commands.DeactivateWelfareGroup;
using AnSinhSo.Application.WelfareGroups.Queries.GetWelfareGroupById;
using AnSinhSo.Application.WelfareGroups.Queries.GetWelfareGroupList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/welfare-groups")]
public class WelfareGroupsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WelfareGroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] GetWelfareGroupListQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetWelfareGroupByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWelfareGroupCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}/name")]
    public async Task<IActionResult> ChangeName(Guid id, [FromBody] ChangeWelfareGroupNameCommand command)
    {
        var request = command with { WelfareGroupId = id };
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("{id}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var command = new ActivateWelfareGroupCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var command = new DeactivateWelfareGroupCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
