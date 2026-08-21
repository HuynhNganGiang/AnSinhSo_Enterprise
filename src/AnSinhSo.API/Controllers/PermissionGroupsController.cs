using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.Queries.GetPermissionGroups;
using AnSinhSo.Contracts.Authorization;
using AnSinhSo.Domain.Constants;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/permission-groups")]
[Tags("Permission Groups")]
public sealed class PermissionGroupsController : ApiControllerBase
{
    private readonly ISender _sender;

    public PermissionGroupsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Lấy danh sách các phân nhóm quyền hạn được định nghĩa sẵn trong hệ thống.
    /// </summary>
    /// <remarks>
    /// Hệ thống trả về danh sách các nhóm quyền đã được cấu hình để tiện lợi cho việc gán quyền hàng loạt. Phân nhóm giúp quản lý bảo mật một cách có hệ thống và khoa học hơn.
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
    [Authorize(Policy = Permissions.PermissionsModule.View)]
    public async Task<IActionResult> GetPermissionGroups(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPermissionGroupsQuery(), cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult<System.Collections.Generic.IReadOnlyCollection<AnSinhSo.Application.Authorization.DTOs.PermissionGroupWithPermissionsDto>>.SuccessResult(result.Value));
    }
}
