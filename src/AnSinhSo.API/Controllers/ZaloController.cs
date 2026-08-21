using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Zalo.Commands.AuthenticateZaloUser;
using AnSinhSo.Application.Zalo.Commands.HandleZaloWebhook;
using AnSinhSo.Application.Zalo.Commands.LinkZaloUser;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnSinhSo.Application.Zalo;
namespace AnSinhSo.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/zalo")]
public sealed class ZaloController : ApiControllerBase
{
    private readonly ISender _sender;

    public ZaloController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Xác thực người dùng qua hệ thống Zalo
    /// </summary>
    /// <remarks>
    /// Xử lý đăng nhập hoặc đồng bộ thông tin từ Zalo. Trả về mã truy cập nếu tài khoản Zalo đã được liên kết với hệ thống.
    /// </remarks>
    /// <param name="request">Thông tin xác thực (Mã xác quyền từ Zalo)</param>
    /// <param name="cancellationToken">Thẻ hủy tác vụ</param>
    [HttpPost("auth")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AuthenticateZaloUser([FromBody] AuthenticateZaloUserRequest request, CancellationToken cancellationToken)
    {
        var command = new AuthenticateZaloUserCommand(
            request.AuthorizationCode,
            HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            Request.Headers["User-Agent"].ToString(),
            request.DeviceName ?? "Unknown Device"
        );

        var result = await _sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Liên kết tài khoản Zalo với tài khoản công dân
    /// </summary>
    /// <remarks>
    /// Dành cho trường hợp người dùng đăng nhập qua hệ thống Zalo nhưng chưa liên kết tài khoản. Yêu cầu truyền mã định danh công dân hợp lệ.
    /// </remarks>
    /// <param name="request">Thông tin liên kết</param>
    /// <param name="cancellationToken">Thẻ hủy tác vụ</param>
    [HttpPost("link")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LinkZaloUser([FromBody] LinkZaloUserRequest request, CancellationToken cancellationToken)
    {
        var command = new LinkZaloUserCommand(
            request.ZaloUserId,
            request.CitizenIdentityId
        );

        var result = await _sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    /// <summary>
    /// Nhận sự kiện tự động từ hệ thống Zalo
    /// </summary>
    /// <remarks>
    /// Cập nhật trạng thái người dùng khi có sự kiện từ nền tảng Zalo như quan tâm, bỏ quan tâm hoặc gửi tin nhắn.
    /// </remarks>
    /// <param name="payload">Nội dung dữ liệu từ hệ thống Zalo</param>
    /// <param name="zaloOAService">Dịch vụ kết nối Zalo</param>
    /// <param name="cancellationToken">Thẻ hủy tác vụ</param>
    [HttpPost("webhook")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Webhook([FromBody] JsonElement payload, [FromServices] IZaloOAService zaloOAService, CancellationToken cancellationToken)
    {
        var mac = Request.Headers["X-ZEngine-Mac"].ToString();
        var timestamp = Request.Headers["X-ZEngine-Timestamp"].ToString();
        var appId = payload.TryGetProperty("app_id", out var appIdElement) ? appIdElement.GetString() : string.Empty;
        var rawPayload = payload.GetRawText();
        
        if (string.IsNullOrEmpty(mac) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(appId))
        {
            return BadRequest("Thiếu thông tin xác thực");
        }
        
        if (!zaloOAService.VerifyWebhookSignature(appId, timestamp, mac, rawPayload))
        {
            return BadRequest("Xác thực chữ ký không hợp lệ");
        }

        var eventName = payload.TryGetProperty("event_name", out var eventNameElement) ? eventNameElement.GetString() : string.Empty;
        var sender = payload.TryGetProperty("sender", out var senderElement) ? senderElement : default;
        var zaloUserId = sender.ValueKind != JsonValueKind.Undefined && sender.TryGetProperty("id", out var idElement) ? idElement.GetString() : string.Empty;

        if (string.IsNullOrEmpty(eventName) || string.IsNullOrEmpty(zaloUserId))
        {
            return Ok(); // Acknowledge with OK to prevent Zalo from retrying invalid payloads
        }

        var command = new HandleZaloWebhookCommand(eventName, zaloUserId, payload);
        await _sender.Send(command, cancellationToken);
        
        return Ok();
    }

    /// <summary>
    /// Kiểm tra kết nối với hệ thống Zalo
    /// </summary>
    /// <remarks>
    /// API dùng để xác nhận ứng dụng có thể giao tiếp thành công với hệ thống Zalo.
    /// </remarks>
    /// <param name="zaloOAService">Dịch vụ kết nối Zalo</param>
    /// <param name="cancellationToken">Thẻ hủy tác vụ</param>
    [HttpGet("test-connection")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> TestConnection([FromServices] IZaloOAService zaloOAService, CancellationToken cancellationToken)
    {
        var isConnected = await zaloOAService.TestConnectionAsync(cancellationToken);
        if (isConnected)
        {
            return Ok(new { Message = "Kết nối đến hệ thống Zalo thành công" });
        }
        
        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Không thể kết nối đến hệ thống Zalo" });
    }
}

public class AuthenticateZaloUserRequest
{
    public string AuthorizationCode { get; set; } = string.Empty;
    public string? DeviceName { get; set; }
}

public class LinkZaloUserRequest
{
    public string ZaloUserId { get; set; } = string.Empty;
    public Guid CitizenIdentityId { get; set; }
}
