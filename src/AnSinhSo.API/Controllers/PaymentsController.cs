using System;
using System.Threading.Tasks;
using AnSinhSo.Application.Payments.Commands.ApprovePayment;
using AnSinhSo.Application.Payments.Commands.CancelPayment;
using AnSinhSo.Application.Payments.Commands.CompletePayment;
using AnSinhSo.Application.Payments.Commands.CreatePayment;
using AnSinhSo.Application.Payments.Commands.FailPayment;
using AnSinhSo.Application.Payments.DTOs;
using AnSinhSo.Application.Payments.Queries.GetCitizenPayments;
using AnSinhSo.Application.Payments.Queries.GetHouseholdPayments;
using AnSinhSo.Application.Payments.Queries.GetPaymentById;
using AnSinhSo.Application.Payments.Queries.GetPendingPayments;
using AnSinhSo.Application.Payments.Queries.SearchPayments;
using AnSinhSo.Domain.Constants;
using AnSinhSo.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers;

[ApiController]
[Route("api/v1/payments")]
[Tags("Payment Management")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly ISender _sender;

    public PaymentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Payments.Create)]
    [ProducesResponseType(typeof(ApiResult<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command)
    {
        var result = await _sender.Send(command);
        return result.IsSuccess 
            ? Ok(ApiResult<Guid>.SuccessResult(result.Value, "Payment created successfully."))
            : BadRequest(ApiResult<Guid>.FailureResult(result.Error.Message));
    }

    [HttpPut("{id}/approve")]
    [Authorize(Policy = Permissions.Payments.Approve)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Approve(Guid id)
    {
        var result = await _sender.Send(new ApprovePaymentCommand(id));
        return result.IsSuccess 
            ? Ok(ApiResult<bool>.SuccessResult(true, "Payment approved successfully."))
            : BadRequest(ApiResult<bool>.FailureResult(result.Error.Message));
    }

    [HttpPut("{id}/complete")]
    [Authorize(Policy = Permissions.Payments.Complete)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Complete(Guid id, [FromBody] DateTime actualPaymentDate)
    {
        var result = await _sender.Send(new CompletePaymentCommand(id, actualPaymentDate));
        return result.IsSuccess 
            ? Ok(ApiResult<bool>.SuccessResult(true, "Payment completed successfully."))
            : BadRequest(ApiResult<bool>.FailureResult(result.Error.Message));
    }

    [HttpPut("{id}/fail")]
    [Authorize(Policy = Permissions.Payments.Complete)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Fail(Guid id, [FromBody] string reason)
    {
        var result = await _sender.Send(new FailPaymentCommand(id, reason));
        return result.IsSuccess 
            ? Ok(ApiResult<bool>.SuccessResult(true, "Payment marked as failed."))
            : BadRequest(ApiResult<bool>.FailureResult(result.Error.Message));
    }

    [HttpPut("{id}/cancel")]
    [Authorize(Policy = Permissions.Payments.Cancel)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] string reason)
    {
        var result = await _sender.Send(new CancelPaymentCommand(id, reason));
        return result.IsSuccess 
            ? Ok(ApiResult<bool>.SuccessResult(true, "Payment cancelled successfully."))
            : BadRequest(ApiResult<bool>.FailureResult(result.Error.Message));
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Permissions.Payments.View)]
    [ProducesResponseType(typeof(ApiResult<PaymentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sender.Send(new GetPaymentByIdQuery(id));
        return result.IsSuccess 
            ? Ok(ApiResult<PaymentDto>.SuccessResult(result.Value))
            : NotFound(ApiResult<PaymentDto>.FailureResult(result.Error.Message));
    }

    [HttpGet]
    [Authorize(Policy = Permissions.Payments.View)]
    [ProducesResponseType(typeof(ApiResult<AnSinhSo.Contracts.Common.PagedResult<PaymentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] SearchPaymentRequestDto request)
    {
        var result = await _sender.Send(new SearchPaymentsQuery(request));
        return result.IsSuccess 
            ? Ok(ApiResult<AnSinhSo.Contracts.Common.PagedResult<PaymentDto>>.SuccessResult(result.Value))
            : BadRequest(ApiResult<AnSinhSo.Contracts.Common.PagedResult<PaymentDto>>.FailureResult(result.Error.Message));
    }

    [HttpGet("citizen/{citizenId}")]
    [Authorize(Policy = Permissions.Payments.View)]
    [ProducesResponseType(typeof(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCitizenPayments(Guid citizenId)
    {
        var result = await _sender.Send(new GetCitizenPaymentsQuery(citizenId));
        return result.IsSuccess 
            ? Ok(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>.SuccessResult(result.Value))
            : BadRequest(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>.FailureResult(result.Error.Message));
    }

    [HttpGet("household/{householdId}")]
    [Authorize(Policy = Permissions.Payments.View)]
    [ProducesResponseType(typeof(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHouseholdPayments(Guid householdId)
    {
        var result = await _sender.Send(new GetHouseholdPaymentsQuery(householdId));
        return result.IsSuccess 
            ? Ok(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>.SuccessResult(result.Value))
            : BadRequest(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>.FailureResult(result.Error.Message));
    }

    [HttpGet("pending")]
    [Authorize(Policy = Permissions.Payments.View)]
    [ProducesResponseType(typeof(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendingPayments()
    {
        var result = await _sender.Send(new GetPendingPaymentsQuery());
        return result.IsSuccess 
            ? Ok(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>.SuccessResult(result.Value))
            : BadRequest(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>.FailureResult(result.Error.Message));
    }
}
