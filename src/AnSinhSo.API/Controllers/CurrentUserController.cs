using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.Queries.GetCurrentUserPermissions;
using AnSinhSo.Application.Citizens.Queries.GetCitizenById;
using AnSinhSo.Contracts.Citizens;
using AnSinhSo.Application.Authorization.Queries.GetCurrentUserRoles;
using AnSinhSo.Contracts.Authorization;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/users/me")]
[Tags("Current User")]
public sealed class CurrentUserController : ApiControllerBase
{
    private readonly ISender _sender;
    private readonly AnSinhSo.Domain.Interfaces.ICurrentUser _currentUser;

    public CurrentUserController(ISender sender, AnSinhSo.Domain.Interfaces.ICurrentUser currentUser)
    {
        _sender = sender;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Truy xuất toàn bộ thông tin chi tiết của người dùng đang đăng nhập vào hệ thống.
    /// </summary>
    /// <remarks>
    /// Chức năng này cung cấp cái nhìn tổng quan về hồ sơ cá nhân và trạng thái tài khoản của người dùng. Dữ liệu trả về được lấy trực tiếp từ phiên làm việc hiện tại để đảm bảo độ chính xác.
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
    [HttpGet("profile")]
    [Authorize] // Requires basic authentication
    [ProducesResponseType(typeof(AnSinhSo.Shared.Responses.ApiResult<CitizenDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        if (!System.Guid.TryParse(_currentUser.UserId, out var citizenIdentityId))
        {
            return Unauthorized();
        }

        // Pending: In a real scenario, we might need to map CitizenIdentityId to CitizenId
        // but since we are using existing GetCitizenByIdQuery, we assume CitizenId is passed or
        // we need another query GetCitizenByIdentityIdQuery. Wait, AD #100 says CitizenIdentity maps to Citizen.
        // Let's create a new query `GetCitizenByCitizenIdentityIdQuery`? No, the correction says:
        // "Correction: Use existing GetCitizenByIdQuery for CurrentUserController instead of creating a new query."
        // We will query the DB for the CitizenId if needed.
        
        // Wait, how to get CitizenId from ICurrentUser?
        var identityResult = await _sender.Send(new AnSinhSo.Application.Authorization.Queries.GetCurrentUserProfile.GetCurrentUserProfileQuery(), cancellationToken);
        if(identityResult.IsFailure) return HandleFailure(identityResult);

        var result = await _sender.Send(new GetCitizenByIdQuery(identityResult.Value.Id), cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult<CitizenDetailDto>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Liệt kê toàn bộ các vai trò mà người dùng hiện tại đang được phân công đảm nhiệm.
    /// </summary>
    /// <remarks>
    /// Hệ thống sẽ kiểm tra và trả về danh sách các vai trò chức năng gắn liền với tài khoản đang đăng nhập. Điều này giúp xác định phạm vi hoạt động của người dùng trên toàn hệ thống.
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
    [HttpGet("roles")]
    [Authorize]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCurrentUserRolesQuery(), cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult<System.Collections.Generic.List<RoleDto>>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Liệt kê chi tiết danh sách các quyền hạn thao tác mà người dùng hiện tại được phép thực hiện.
    /// </summary>
    /// <remarks>
    /// Kết quả trả về là tập hợp tất cả các hành động mà người dùng có thể làm đối với dữ liệu hệ thống. Tính năng này được dùng để quyết định việc hiển thị các nút chức năng trên giao diện.
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
    [HttpGet("permissions")]
    [Authorize]
    public async Task<IActionResult> GetPermissions(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCurrentUserPermissionsQuery(), cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(AnSinhSo.Shared.Responses.ApiResult<System.Collections.Generic.List<string>>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Truy xuất danh sách các thông báo hệ thống được gửi đến tài khoản của người dùng hiện tại.
    /// </summary>
    /// <remarks>
    /// Chức năng này liệt kê các tin tức, cập nhật hoặc cảnh báo liên quan trực tiếp đến tài khoản người dùng. Các thông báo được sắp xếp theo thời gian để người dùng dễ dàng theo dõi.
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
    [HttpGet("notifications")]
    [Authorize]
    public async Task<IActionResult> GetNotifications(CancellationToken cancellationToken)
    {
        var identityResult = await _sender.Send(new AnSinhSo.Application.Authorization.Queries.GetCurrentUserProfile.GetCurrentUserProfileQuery(), cancellationToken);
        if (identityResult.IsFailure) return HandleFailure(identityResult);

        var result = await _sender.Send(new AnSinhSo.Application.Notifications.Queries.GetUserNotifications.GetUserNotificationsQuery(identityResult.Value.Id), cancellationToken);
        
        return Ok(AnSinhSo.Shared.Responses.ApiResult<System.Collections.Generic.List<AnSinhSo.Application.Notifications.DTOs.NotificationDto>>.SuccessResult(result));
    }

    /// <summary>
    /// Đếm và trả về tổng số lượng các thông báo mà người dùng hiện tại chưa bấm xem.
    /// </summary>
    /// <remarks>
    /// Hệ thống sẽ tổng hợp số lượng thông báo mới để hiển thị cảnh báo trên biểu tượng thông báo của ứng dụng. Điều này giúp người dùng không bỏ lỡ các thông tin quan trọng.
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
    [HttpGet("unread-count")]
    [Authorize]
    public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken)
    {
        var identityResult = await _sender.Send(new AnSinhSo.Application.Authorization.Queries.GetCurrentUserProfile.GetCurrentUserProfileQuery(), cancellationToken);
        if (identityResult.IsFailure) return HandleFailure(identityResult);

        var result = await _sender.Send(new AnSinhSo.Application.Notifications.Queries.GetUnreadNotificationCount.GetUnreadNotificationCountQuery(identityResult.Value.Id), cancellationToken);
        
        return Ok(AnSinhSo.Shared.Responses.ApiResult<AnSinhSo.Application.Notifications.DTOs.UnreadCountDto>.SuccessResult(result));
    }
}
