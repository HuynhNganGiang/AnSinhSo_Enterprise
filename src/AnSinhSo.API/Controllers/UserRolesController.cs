using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.Commands.AssignRole;
using AnSinhSo.Application.Authorization.Commands.RevokeRole;
using AnSinhSo.Application.Authorization.Queries.GetUserRoles;
using AnSinhSo.Contracts.Authorization;
using AnSinhSo.Domain.Constants;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/users")]
[Tags("User Roles")]
public sealed class UserRolesController : ApiControllerBase
{
    private readonly ISender _sender;

    public UserRolesController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Liệt kê danh sách các quyền hạn và chức danh đã được gán cho một cá nhân trong hệ thống.
    /// </summary>
    /// <remarks>
    /// Truy vấn thông tin tổng hợp về quyền hạn của một cán bộ cụ thể để phục vụ công tác kiểm tra chéo. Thông tin này rất quan trọng để đảm bảo không có ai được cấp quyền vượt cấp.
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
    [HttpGet("{id:guid}/roles")]
    [Authorize(Policy = Permissions.UserRoles.View)]
    public async Task<IActionResult> GetUserRoles(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetUserRolesQuery(id), cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult<System.Collections.Generic.IReadOnlyCollection<AnSinhSo.Application.Authorization.DTOs.RoleDto>>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Bổ sung một chức danh mới cho người dùng để cấp thêm các quyền hạn tương ứng với công việc.
    /// </summary>
    /// <remarks>
    /// Thao tác này giao thêm nhiệm vụ và quyền truy cập dữ liệu cho một tài khoản cán bộ trên hệ thống. Đây là quy trình bắt buộc khi có sự luân chuyển công tác hoặc thăng tiến.
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
    [HttpPost("{id:guid}/roles")]
    [Authorize(Policy = Permissions.UserRoles.Manage)]
    public async Task<IActionResult> AssignRole(Guid id, [FromBody] AssignRoleRequest request, CancellationToken cancellationToken)
    {
        var command = new AssignRoleCommand(id, request.RoleId);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult.SuccessResult("Role assigned successfully."));
    }

    /// <summary>
    /// Gỡ bỏ một chức danh đã được gán của người dùng nhằm giới hạn lại quyền hạn truy cập.
    /// </summary>
    /// <remarks>
    /// Tính năng này rút lại quyền lợi truy cập của một cá nhân đối với các chức năng nhạy cảm. Thường được sử dụng khi nhân viên chuyển bộ phận hoặc chấm dứt hợp đồng lao động.
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
    [HttpDelete("{id:guid}/roles/{roleId:guid}")]
    [Authorize(Policy = Permissions.UserRoles.Manage)]
    public async Task<IActionResult> RevokeRole(Guid id, Guid roleId, CancellationToken cancellationToken)
    {
        var command = new RevokeRoleCommand(id, roleId);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent(); // Idempotent DELETE
    }
}
