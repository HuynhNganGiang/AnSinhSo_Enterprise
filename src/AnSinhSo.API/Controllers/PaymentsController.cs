using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AnSinhSo.Application.Payments.Commands.CreatePayment;
using AnSinhSo.Application.Payments.Commands.ClosePayment;
using AnSinhSo.Application.Payments.Commands.CancelPayment;
using AnSinhSo.Application.Payments.Queries.GetPaymentById;
using AnSinhSo.Application.Payments.Queries.GetPaymentList;

namespace AnSinhSo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaymentList([FromQuery] GetPaymentListQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPaymentById(Guid id)
    {
        var result = await _mediator.Send(new GetPaymentByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}/close")]
    public async Task<IActionResult> ClosePayment(Guid id)
    {
        var result = await _mediator.Send(new ClosePaymentCommand(id));
        return Ok(result);
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelPayment(Guid id)
    {
        var result = await _mediator.Send(new CancelPaymentCommand(id));
        return Ok(result);
    }
}
