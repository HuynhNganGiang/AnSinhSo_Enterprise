using System;
using System.Threading.Tasks;
using AnSinhSo.Application.Policies.Commands.ActivatePolicy;
using AnSinhSo.Application.Policies.Commands.CreatePolicy;
using AnSinhSo.Application.Policies.Commands.DeactivatePolicy;
using AnSinhSo.Application.Policies.Commands.UpdatePolicyAmount;
using AnSinhSo.Application.Policies.Queries.GetPolicyById;
using AnSinhSo.Application.Policies.Queries.GetPolicyList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/policies")]
public class PoliciesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PoliciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] GetPolicyListQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetPolicyByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePolicyCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}/amount")]
    public async Task<IActionResult> UpdateAmount(Guid id, [FromBody] UpdatePolicyAmountCommand command)
    {
        var request = command with { PolicyId = id };
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("{id}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var command = new ActivatePolicyCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var command = new DeactivatePolicyCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
