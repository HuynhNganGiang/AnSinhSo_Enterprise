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
    /// Tiếp nhận thông tin và tạo mới một hồ sơ định danh công dân trên hệ thống.
    /// </summary>
    /// <remarks>
    /// Chức năng này hỗ trợ cán bộ tiếp nhận yêu cầu đăng ký định danh dựa trên căn cước công dân và số điện thoại. Hồ sơ sau khi tạo sẽ ở trạng thái chờ duyệt để tiếp tục các bước xác thực.
    /// </remarks>
    /// <response code="200">
    /// Success Response
    /// {
    ///   "success": true,
    ///   "message": "Thao tác thành công.",
    ///   "data": { }
    /// }
    /// </response>
    /// <response code="400">
    /// Validation Error
    /// {
    ///   "success": false,
    ///   "message": "Dữ liệu đầu vào không hợp lệ.",
    ///   "errors": [ ]
    /// }
    /// </response>
    /// <response code="401">Unauthorized - Người dùng chưa đăng nhập.</response>
    /// <response code="403">Forbidden - Không có quyền truy cập.</response>
    /// <response code="404">Not Found - Không tìm thấy dữ liệu.</response>
    /// <response code="500">Internal Server Error - Lỗi hệ thống.</response>
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
    /// Phát sinh và gửi mã xác thực dùng một lần qua tin nhắn văn bản cho người dân.
    /// </summary>
    /// <remarks>
    /// Hệ thống sẽ tạo ra một mã ngẫu nhiên và gửi đến số điện thoại đã đăng ký của công dân. Thao tác này đảm bảo tính nhất quán, không tạo mã mới nếu mã trước đó vẫn còn hiệu lực.
    /// </remarks>
    /// <response code="200">
    /// Success Response
    /// {
    ///   "success": true,
    ///   "message": "Thao tác thành công.",
    ///   "data": { }
    /// }
    /// </response>
    /// <response code="400">
    /// Validation Error
    /// {
    ///   "success": false,
    ///   "message": "Dữ liệu đầu vào không hợp lệ.",
    ///   "errors": [ ]
    /// }
    /// </response>
    /// <response code="401">Unauthorized - Người dùng chưa đăng nhập.</response>
    /// <response code="403">Forbidden - Không có quyền truy cập.</response>
    /// <response code="404">Not Found - Không tìm thấy dữ liệu.</response>
    /// <response code="500">Internal Server Error - Lỗi hệ thống.</response>
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
    /// Kiểm tra và xác nhận mã xác thực do người dân cung cấp để hoàn tất việc định danh.
    /// </summary>
    /// <remarks>
    /// Quá trình này sẽ đối chiếu mã xác thực mà người dân nhập vào với mã đã được lưu trữ trước đó. Nếu thông tin hoàn toàn khớp nhau, hồ sơ của người dân sẽ chính thức được hệ thống chấp nhận.
    /// </remarks>
    /// <response code="200">
    /// Success Response
    /// {
    ///   "success": true,
    ///   "message": "Thao tác thành công.",
    ///   "data": { }
    /// }
    /// </response>
    /// <response code="400">
    /// Validation Error
    /// {
    ///   "success": false,
    ///   "message": "Dữ liệu đầu vào không hợp lệ.",
    ///   "errors": [ ]
    /// }
    /// </response>
    /// <response code="401">Unauthorized - Người dùng chưa đăng nhập.</response>
    /// <response code="403">Forbidden - Không có quyền truy cập.</response>
    /// <response code="404">Not Found - Không tìm thấy dữ liệu.</response>
    /// <response code="500">Internal Server Error - Lỗi hệ thống.</response>
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
