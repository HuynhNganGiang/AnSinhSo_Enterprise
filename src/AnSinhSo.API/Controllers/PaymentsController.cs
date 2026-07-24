using System;
using System.Threading.Tasks;
using AnSinhSo.Application.Payments.Commands.AddPaymentDetail;
using AnSinhSo.Application.Payments.Commands.CancelPayment;
using AnSinhSo.Application.Payments.Commands.ClosePayment;
using AnSinhSo.Application.Payments.Commands.CreatePayment;
using AnSinhSo.Application.Payments.Queries.GetPaymentById;
using AnSinhSo.Application.Payments.Queries.GetPaymentList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] GetPaymentListQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetPaymentByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/details")]
    public async Task<IActionResult> AddDetail(Guid id, [FromBody] AddPaymentDetailCommand command)
    {
        var request = command with { PaymentId = id };
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var command = new CancelPaymentCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/close")]
    public async Task<IActionResult> Close(Guid id)
    {
        var command = new ClosePaymentCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
