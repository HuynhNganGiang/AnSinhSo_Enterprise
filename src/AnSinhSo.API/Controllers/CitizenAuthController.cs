using System.Net;
using AnSinhSo.Application.Authentication.Citizen.RequestOtp;
using AnSinhSo.Application.Authentication.Citizen.VerifyOtp;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace AnSinhSo.API.Controllers;

/// <summary>
/// Quản lý xác thực công dân (Đăng nhập, Đăng ký)
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth/citizen")]
[ApiExplorerSettings(GroupName = "Citizen Authentication")]
public class CitizenAuthController : ApiControllerBase
{
    private readonly ISender _sender;

    public CitizenAuthController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Yêu cầu gửi mã OTP cho công dân
    /// </summary>
    /// <remarks>
    /// Hệ thống sẽ kiểm tra và gửi OTP (qua Zalo OA hoặc SMS). 
    /// Có cơ chế chống spam, giới hạn số lần yêu cầu trong khoảng thời gian nhất định.
    /// </remarks>
    /// <param name="command">Thông tin số điện thoại yêu cầu gửi OTP</param>
    /// <returns>Thông tin RequestId để sử dụng cho bước xác thực</returns>
    /// <response code="200">Gửi OTP thành công</response>
    /// <response code="400">Dữ liệu đầu vào không hợp lệ (lỗi validation)</response>
    /// <response code="429">Yêu cầu quá nhiều lần (spam), cần đợi trước khi yêu cầu lại</response>
    [HttpPost("request-otp")]
    [AllowAnonymous]
    [ServiceFilter(typeof(AnSinhSo.API.Filters.CitizenOtpAntiSpamFilter))]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> RequestOtp([FromBody] RequestCitizenOtpCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        return Ok(result.Value);
    }

    /// <summary>
    /// Xác thực mã OTP của công dân
    /// </summary>
    /// <remarks>
    /// Sử dụng RequestId nhận được từ bước gửi OTP và mã OTP để xác thực.
    /// Nếu số điện thoại chưa có tài khoản, trạng thái sẽ là "PendingApproval" (Chờ duyệt).
    /// Nếu số điện thoại đã có tài khoản, trả về Token (Access Token, Refresh Token) để đăng nhập.
    /// </remarks>
    /// <param name="command">Thông tin RequestId và mã OTP</param>
    /// <returns>Trạng thái xác thực và Access Token nếu đăng nhập thành công</returns>
    /// <response code="200">Xác thực thành công (có Token hoặc yêu cầu tạo mới tài khoản)</response>
    /// <response code="400">OTP không hợp lệ hoặc đã hết hạn</response>
    [HttpPost("verify-otp")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(VerifyCitizenOtpResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyCitizenOtpCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        return Ok(result.Value);
    }
}
