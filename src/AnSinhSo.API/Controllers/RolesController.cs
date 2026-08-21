using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.Commands.CreateRole;
using AnSinhSo.Application.Authorization.Commands.DeleteRole;
using AnSinhSo.Application.Authorization.Commands.UpdateRole;
using AnSinhSo.Application.Authorization.Commands.UpdateRolePermissions;
using AnSinhSo.Application.Authorization.Queries.GetRoleDetail;
using AnSinhSo.Application.Authorization.Queries.GetRoles;
using AnSinhSo.Contracts.Authorization;
using AnSinhSo.Domain.Constants;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/roles")]
[Tags("Roles")] // AD #122
public sealed class RolesController : ApiControllerBase
{
    private readonly ISender _sender;

    public RolesController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Truy vấn danh sách tất cả các chức danh người dùng đang tồn tại trên toàn bộ hệ thống.
    /// </summary>
    /// <remarks>
    /// Kết quả trả về danh mục các chức danh được định nghĩa sẵn phục vụ công tác phân luồng công việc. Cán bộ quản trị dùng danh sách này để quản lý và cấu hình tài khoản.
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
    [HttpGet]
    [Authorize(Policy = Permissions.Roles.View)]
    [ProducesResponseType(typeof(RoleDto[]), StatusCodes.Status200OK)] // Simplified for brevity, usually wrapped in Envelope/Pagination
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetRolesQuery(), cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        // According to AD #116, should return ApiResult envelope. We will assume ApiControllerBase handles it or we wrap it here.
        // Actually, the user asked for ApiResult<T>. So:
        return Ok(AnSinhSo.Shared.Responses.ApiResult<System.Collections.Generic.IReadOnlyCollection<AnSinhSo.Application.Authorization.DTOs.RoleDto>>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Truy xuất chi tiết thông tin và cấu hình bảo mật của một chức danh người dùng cụ thể.
    /// </summary>
    /// <remarks>
    /// Hiển thị đầy đủ thông tin về một chức danh bao gồm mô tả và danh sách các quyền hạn đang được đính kèm. Điều này giúp kiểm toán bảo mật và rà soát phân quyền một cách dễ dàng.
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
    [HttpGet("{id:guid}")]
    [Authorize(Policy = Permissions.Roles.View)]
    public async Task<IActionResult> GetRoleById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetRoleDetailQuery(id), cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult<AnSinhSo.Application.Authorization.DTOs.RoleDetailDto>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Thêm mới một chức danh người dùng với các thiết lập quyền hạn tùy chỉnh trên hệ thống.
    /// </summary>
    /// <remarks>
    /// Cho phép hệ thống linh hoạt tạo ra các chức danh mới để đáp ứng yêu cầu phân công công việc thực tế. Sau khi tạo, chức danh này có thể lập tức được cấp phát cho các nhân viên.
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
    [Authorize(Policy = Permissions.Roles.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateRoleCommand(request.Name, request.Description, false);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(nameof(GetRoleById), new { id = result.Value }, AnSinhSo.Shared.Responses.ApiResult<Guid>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Sửa đổi thông tin cơ bản của một chức danh người dùng đã được tạo trước đó.
    /// </summary>
    /// <remarks>
    /// Thay đổi tên gọi hoặc mô tả của chức danh để phản ánh đúng thực tế nhiệm vụ của cán bộ. Các thay đổi này không làm ảnh hưởng đến những người đang giữ chức danh đó.
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
    [HttpPut("{id:guid}")]
    [Authorize(Policy = Permissions.Roles.Update)]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateRoleCommand(id, request.Name, request.Description);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult.SuccessResult("Role updated successfully."));
    }

    /// <summary>
    /// Xóa vĩnh viễn một chức danh người dùng khỏi hệ thống nếu không còn nhu cầu sử dụng.
    /// </summary>
    /// <remarks>
    /// Hành động này loại bỏ hoàn toàn cấu hình chức danh ra khỏi cơ sở dữ liệu để làm gọn hệ thống. Chỉ cho phép thực hiện nếu không có bất kỳ ai đang mang chức danh này.
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
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = Permissions.Roles.Delete)]
    public async Task<IActionResult> DeleteRole(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteRoleCommand(id);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent(); // Idempotent DELETE returns 204
    }

    /// <summary>
    /// Điều chỉnh danh sách các quyền hạn thao tác được phép thực hiện của một chức danh cụ thể.
    /// </summary>
    /// <remarks>
    /// Tính năng quản trị cấp cao cho phép thay đổi phạm vi hoạt động của một nhóm người dùng cùng lúc. Bất kỳ thay đổi nào cũng sẽ lập tức có tác dụng đối với tất cả tài khoản mang chức danh đó.
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
    [HttpPut("{id:guid}/permissions")]
    [Authorize(Policy = Permissions.PermissionsModule.Manage)]
    public async Task<IActionResult> UpdateRolePermissions(Guid id, [FromBody] UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateRolePermissionsCommand(id, request.PermissionIds);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult.SuccessResult("Permissions synchronized successfully."));
    }
}
