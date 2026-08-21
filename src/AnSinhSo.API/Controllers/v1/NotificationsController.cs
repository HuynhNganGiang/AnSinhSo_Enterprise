using System;
using System.Threading.Tasks;
using AnSinhSo.Application.Notifications.Commands.BroadcastNotification;
using AnSinhSo.Application.Notifications.Commands.CreateNotification;
using AnSinhSo.Application.Notifications.Commands.MarkNotificationAsRead;
using AnSinhSo.Application.Notifications.Commands.RetryNotification;
using AnSinhSo.Application.Notifications.Queries.GetNotificationById;
using AnSinhSo.Application.Notifications.Queries.GetNotifications;
using AnSinhSo.Application.Notifications.Queries.GetNotificationStatistics;
using AnSinhSo.Application.Notifications.Queries.GetUnreadNotifications;
using AnSinhSo.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AnSinhSo.API.Controllers;

namespace AnSinhSo.Api.Controllers.v1;

[Authorize]
[ApiController]
[Route("api/v1/notifications")]
public class NotificationsController : ApiControllerBase
{
    private readonly ISender _sender;

    public NotificationsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Truy xuất toàn bộ các tin tức và cảnh báo hệ thống đã được giao dịch qua lại theo thời gian.
    /// </summary>
    /// <remarks>
    /// Người sử dụng có thể xem lại lịch sử các sự kiện hoặc tin nhắn đã được hệ thống lưu lại. Dữ liệu trả về luôn sắp xếp tin nhắn mới nhất lên đầu để thuận tiện việc theo dõi.
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
    [Authorize(Policy = Permissions.Notifications.View)]
    public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsQuery query)
    {
        var result = await _sender.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Liệt kê danh sách những thông điệp mới mà người sử dụng hiện tại chưa từng bấm vào để xem nội dung.
    /// </summary>
    /// <remarks>
    /// Hệ thống cung cấp chức năng này để lọc riêng các tin nhắn quan trọng cần phải xử lý ngay lập tức. Đây là một cơ chế giúp nâng cao hiệu suất phản hồi công việc của cán bộ.
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
    [HttpGet("unread")]
    [Authorize(Policy = Permissions.Notifications.View)]
    public async Task<IActionResult> GetUnreadNotifications()
    {
        var result = await _sender.Send(new GetUnreadNotificationsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Cung cấp báo cáo số liệu tổng quát về tình trạng gửi nhận và tỷ lệ tương tác của các thông điệp.
    /// </summary>
    /// <remarks>
    /// Chức năng này giúp bộ phận truyền thông đánh giá được hiệu quả tiếp cận của các thông báo được phát hành. Số liệu thống kê được biểu diễn bằng các tiêu chí như số lượng đã xem và số bị lỗi gửi.
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
    [HttpGet("statistics")]
    [Authorize(Policy = Permissions.Notifications.Statistics)]
    public async Task<IActionResult> GetNotificationStatistics()
    {
        var result = await _sender.Send(new GetNotificationStatisticsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Hiển thị đầy đủ nội dung bài viết và các tệp tài liệu đính kèm của một thông điệp cụ thể.
    /// </summary>
    /// <remarks>
    /// Người sử dụng gọi tính năng này khi cần đọc chi tiết toàn văn bản hướng dẫn hoặc quyết định hành chính. Hệ thống tự động ghi nhận sự kiện mở tài liệu để đánh dấu thư đã được đọc.
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
    [HttpGet("{id}")]
    [Authorize(Policy = Permissions.Notifications.View)]
    public async Task<IActionResult> GetNotificationById(Guid id)
    {
        var result = await _sender.Send(new GetNotificationByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    /// <summary>
    /// Soạn thảo và lưu trữ một bản tin mới trên hệ thống để chuẩn bị phát hành rộng rãi cho người dân.
    /// </summary>
    /// <remarks>
    /// Cán bộ truyền thông sử dụng công cụ này để biên soạn các nội dung cảnh báo thiên tai hoặc thay đổi chính sách. Quá trình tạo mới hỗ trợ đính kèm tệp và định dạng văn bản nâng cao.
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
    [Authorize(Policy = Permissions.Notifications.Create)]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationCommand command)
    {
        var id = await _sender.Send(command);
        return CreatedAtAction(nameof(GetNotificationById), new { id = id }, null);
    }

    /// <summary>
    /// Phát hành một văn bản cùng lúc đến nhiều đối tượng tiếp nhận khác nhau trên diện rộng của toàn hệ thống.
    /// </summary>
    /// <remarks>
    /// Đây là chức năng quan trọng dùng để truyền đạt các chỉ thị khẩn cấp hoặc tin tức quan trọng đến hàng ngàn tài khoản. Hệ thống sử dụng công nghệ xử lý nền để gửi đi số lượng lớn mà không làm treo máy.
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
    [HttpPost("broadcast")]
    [Authorize(Policy = Permissions.Notifications.Broadcast)]
    public async Task<IActionResult> BroadcastNotification([FromBody] BroadcastNotificationCommand command)
    {
        var count = await _sender.Send(command);
        return Ok(new { Count = count });
    }

    /// <summary>
    /// Phát hành một thông điệp hoặc quyết định cụ thể đến một cá nhân nhận đã được lựa chọn từ trước.
    /// </summary>
    /// <remarks>
    /// Chức năng này chủ yếu dùng để trả kết quả thủ tục hành chính hoặc giải quyết khiếu nại cho từng người dân đơn lẻ. Đảm bảo tính riêng tư và bảo mật tuyệt đối cho nội dung tin nhắn.
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
    [HttpPost("{id}/send")]
    [Authorize(Policy = Permissions.Notifications.Create)]
    public async Task<IActionResult> SendNotification(Guid id)
    {
        await _sender.Send(new AnSinhSo.Application.Notifications.Commands.SendNotification.SendNotificationCommand(id));
        return Ok();
    }

    /// <summary>
    /// Cập nhật trạng thái của bức thư thành đã mở xem sau khi người sử dụng click vào đọc chi tiết.
    /// </summary>
    /// <remarks>
    /// Thao tác này loại bỏ biểu tượng cảnh báo thư mới trên giao diện màn hình của người sử dụng. Dữ liệu trạng thái sẽ được đồng bộ lên máy chủ để tính toán tỷ lệ tương tác.
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
    [HttpPatch("{id}/read")]
    [Authorize(Policy = Permissions.Notifications.Read)]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        await _sender.Send(new MarkNotificationAsReadCommand(id));
        return NoContent();
    }

    /// <summary>
    /// Thực hiện quy trình phát hành lại đối với một thông điệp đã gặp sự cố máy chủ trong lần gửi trước.
    /// </summary>
    /// <remarks>
    /// Cán bộ sử dụng công cụ này khi phát hiện kết nối mạng bị gián đoạn khiến tin nhắn chưa đến được tay người nhận. Hệ thống sẽ cố gắng vượt qua lỗi cũ để đảm bảo thông tin được truyền tải.
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
    [HttpPost("{id}/retry")]
    [Authorize(Policy = Permissions.Notifications.Retry)]
    public async Task<IActionResult> RetryNotification(Guid id)
    {
        await _sender.Send(new RetryNotificationCommand(id));
        return Ok();
    }
}
