using System;
using System.Threading.Tasks;
using AnSinhSo.Application.Policies.Commands.ActivatePolicy;
using AnSinhSo.Application.Policies.Commands.CreatePolicy;
using AnSinhSo.Application.Policies.Commands.DeactivatePolicy;
using AnSinhSo.Application.Policies.Commands.UpdatePolicyAmount;
using AnSinhSo.Application.Policies.Queries.GetPolicyById;
using AnSinhSo.Application.Policies.Queries.GetPolicyList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/policies")]
public class PoliciesController : ControllerBase
{
    private readonly ISender _sender;

    public PoliciesController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Lấy danh sách các danh mục hoặc chính sách hiện hành trên hệ thống dựa trên tiêu chí tìm kiếm.
    /// </summary>
    /// <remarks>
    /// Cung cấp khả năng truy vấn và lọc dữ liệu danh mục để phục vụ công tác tra cứu. Dữ liệu được tổ chức dưới dạng danh sách phân trang để dễ dàng quan sát và lựa chọn.
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
    public async Task<IActionResult> GetList([FromQuery] GetPolicyListQuery query)
    {
        var result = await _sender.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Truy xuất chi tiết cấu hình và thông tin của một danh mục cụ thể được lưu trữ trên hệ thống.
    /// </summary>
    /// <remarks>
    /// Giao diện này hiển thị đầy đủ các trường dữ liệu và thuộc tính cài đặt của một đối tượng quản lý. Nó đặc biệt hữu ích khi cần kiểm tra lại các thiết lập nghiệp vụ quan trọng.
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
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetPolicyByIdQuery(id);
        var result = await _sender.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Thêm mới một cấu hình danh mục hoặc chính sách vào hệ thống an sinh xã hội.
    /// </summary>
    /// <remarks>
    /// Cán bộ quản trị sử dụng tính năng này để khai báo các chính sách hoặc nhóm đối tượng hỗ trợ mới. Dữ liệu mới tạo sẽ được áp dụng cho toàn bộ các quy trình liên quan trong tương lai.
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
    public async Task<IActionResult> Create([FromBody] CreatePolicyCommand command)
    {
        var result = await _sender.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Điều chỉnh và cập nhật lại mức tiền quy định cho một chính sách trợ cấp cụ thể.
    /// </summary>
    /// <remarks>
    /// Tính năng này hỗ trợ cán bộ thay đổi mức chi trả của một chính sách hiện hành cho phù hợp với quy định mới. Sự thay đổi này sẽ được áp dụng ngay lập tức cho các hồ sơ nộp sau thời điểm này.
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
    [HttpPut("{id}/amount")]
    public async Task<IActionResult> UpdateAmount(Guid id, [FromBody] UpdatePolicyAmountCommand command)
    {
        var request = command with { PolicyId = id };
        var result = await _sender.Send(request);
        return Ok(result);
    }

    /// <summary>
    /// Kích hoạt đưa vào sử dụng một chính sách hoặc danh mục đã bị vô hiệu hóa trước đó.
    /// </summary>
    /// <remarks>
    /// Thay đổi trạng thái của bản ghi thành đang hoạt động để hệ thống có thể bắt đầu áp dụng rộng rãi. Các bản ghi đang hoạt động mới được phép hiển thị trên các biểu mẫu lựa chọn.
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
    [HttpPost("{id}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var command = new ActivatePolicyCommand(id);
        var result = await _sender.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Vô hiệu hóa một chính sách hoặc danh mục để ngăn chặn việc tiếp tục sử dụng trong tương lai.
    /// </summary>
    /// <remarks>
    /// Tính năng này dùng để khóa các danh mục đã cũ hoặc không còn hiệu lực thi hành theo pháp luật. Hành động này không làm mất dữ liệu lịch sử nhưng sẽ ẩn danh mục đó đi.
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
    [HttpPost("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var command = new DeactivatePolicyCommand(id);
        var result = await _sender.Send(command);
        return Ok(result);
    }
}
