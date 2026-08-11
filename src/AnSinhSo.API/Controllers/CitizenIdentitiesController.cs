using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.CitizenIdentities.Commands.RegisterCitizenIdentity;
using AnSinhSo.Application.OtpVerifications.Commands.SendOtp;
using AnSinhSo.Application.OtpVerifications.Commands.VerifyOtp;
using AnSinhSo.Contracts.CitizenIdentities;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/citizen-identities")]
[Tags("Citizen Identities")]
[AllowAnonymous]
public sealed class CitizenIdentitiesController : ApiControllerBase
{
    private readonly ISender _sender;

    public CitizenIdentitiesController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Đăng ký định danh công dân mới
    /// </summary>
    /// <remarks>
    /// Tạo hồ sơ định danh Pending. Bắt buộc nhập CCCD hợp lệ và Số ĐT.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(RegisterCitizenIdentityResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterCitizenIdentityRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCitizenIdentityCommand(
            Guid.Parse(request.CitizenId),
            request.PhoneNumber);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        var response = new RegisterCitizenIdentityResponse(result.Value, "PendingVerification");

        return CreatedAtAction(nameof(Register), new { citizenIdentityId = result.Value }, response);
    }

    /// <summary>
    /// Gửi mã OTP xác thực
    /// </summary>
    /// <remarks>
    /// Kích hoạt luồng tạo và gửi OTP qua SMS. Idempotent trong thời gian hiệu lực OTP.
    /// </remarks>
    [HttpPost("{citizenIdentityId:guid}/otp/send")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendOtp(
        [FromRoute] Guid citizenIdentityId,
        [FromBody] SendOtpRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SendOtpCommand(citizenIdentityId, request.PhoneNumber, request.Purpose);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }

    /// <summary>
    /// Xác thực mã OTP
    /// </summary>
    /// <remarks>
    /// Kiểm tra tính hợp lệ của mã OTP. Kích hoạt Identity nếu thành công.
    /// </remarks>
    [HttpPost("{citizenIdentityId:guid}/otp/verify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> VerifyOtp(
        [FromRoute] Guid citizenIdentityId,
        [FromBody] VerifyOtpRequest request,
        CancellationToken cancellationToken)
    {
        var command = new VerifyOtpCommand(citizenIdentityId, request.OtpCode);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }
}
