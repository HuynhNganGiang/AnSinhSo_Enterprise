using System;
using System.Threading.Tasks;
using AnSinhSo.Application.Payments.Commands.ApprovePayment;
using AnSinhSo.Application.Payments.Commands.CancelPayment;
using AnSinhSo.Application.Payments.Commands.CompletePayment;
using AnSinhSo.Application.Payments.Commands.CreatePayment;
using AnSinhSo.Application.Payments.Commands.FailPayment;
using AnSinhSo.Application.Payments.DTOs;
using AnSinhSo.Application.Payments.Queries.GetCitizenPayments;
using AnSinhSo.Application.Payments.Queries.GetHouseholdPayments;
using AnSinhSo.Application.Payments.Queries.GetPaymentById;
using AnSinhSo.Application.Payments.Queries.GetPendingPayments;
using AnSinhSo.Application.Payments.Queries.SearchPayments;
using AnSinhSo.Domain.Constants;
using AnSinhSo.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers;

[ApiController]
[Route("api/v1/payments")]
[Tags("Payment Management")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly ISender _sender;

    public PaymentsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Khởi tạo một yêu cầu chi trả tiền trợ cấp cho công dân hoặc hộ gia đình.
    /// </summary>
    /// <remarks>
    /// Giao diện này cho phép cán bộ tạo một đợt chi trả mới hoàn toàn dựa trên chính sách an sinh đang áp dụng. Thông tin cung cấp cần bao gồm số tiền, đối tượng nhận và thời gian tiến hành.
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
    [Authorize(Policy = Permissions.Payments.Create)]
    [ProducesResponseType(typeof(ApiResult<Guid>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command)
    {
        var result = await _sender.Send(command);
        return result.IsSuccess 
            ? Ok(ApiResult<Guid>.SuccessResult(result.Value, "Payment created successfully."))
            : BadRequest(ApiResult<Guid>.FailureResult(result.Error.Message));
    }

    /// <summary>
    /// Phê duyệt yêu cầu chi trả để chuyển sang giai đoạn tiến hành phát tiền cho người dân.
    /// </summary>
    /// <remarks>
    /// Hành động này được sử dụng bởi cấp quản lý để xem xét và duyệt các yêu cầu tài trợ đã được tạo. Sau khi duyệt, bộ phận kế toán sẽ chuẩn bị ngân sách và lên lịch phát tiền thực tế.
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
    [HttpPut("{id}/approve")]
    [Authorize(Policy = Permissions.Payments.Approve)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Approve(Guid id)
    {
        var result = await _sender.Send(new ApprovePaymentCommand(id));
        return result.IsSuccess 
            ? Ok(ApiResult<bool>.SuccessResult(true, "Payment approved successfully."))
            : BadRequest(ApiResult<bool>.FailureResult(result.Error.Message));
    }

    /// <summary>
    /// Cập nhật trạng thái thành công cho một đợt chi trả sau khi người dân đã nhận đủ tiền.
    /// </summary>
    /// <remarks>
    /// Hệ thống ghi nhận việc chi trả đã được thực hiện xong và số tiền trợ cấp đã đến tay người thụ hưởng. Thông tin này sẽ được lưu trữ vĩnh viễn để phục vụ công tác thanh tra và báo cáo.
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
    [HttpPut("{id}/complete")]
    [Authorize(Policy = Permissions.Payments.Complete)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Complete(Guid id, [FromBody] DateTime actualPaymentDate)
    {
        var result = await _sender.Send(new CompletePaymentCommand(id, actualPaymentDate));
        return result.IsSuccess 
            ? Ok(ApiResult<bool>.SuccessResult(true, "Payment completed successfully."))
            : BadRequest(ApiResult<bool>.FailureResult(result.Error.Message));
    }

    /// <summary>
    /// Ghi nhận một đợt chi trả không thành công kèm theo nguyên nhân cụ thể từ cán bộ.
    /// </summary>
    /// <remarks>
    /// Chức năng này dùng khi quá trình phát tiền gặp sự cố như sai thông tin tài khoản ngân hàng hoặc người nhận vắng mặt. Trạng thái lỗi sẽ giúp các bên liên quan dễ dàng theo dõi và xử lý lại sau.
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
    [HttpPut("{id}/fail")]
    [Authorize(Policy = Permissions.Payments.Complete)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Fail(Guid id, [FromBody] string reason)
    {
        var result = await _sender.Send(new FailPaymentCommand(id, reason));
        return result.IsSuccess 
            ? Ok(ApiResult<bool>.SuccessResult(true, "Payment marked as failed."))
            : BadRequest(ApiResult<bool>.FailureResult(result.Error.Message));
    }

    /// <summary>
    /// Hủy bỏ một đợt chi trả trước khi quá trình phát tiền được diễn ra chính thức.
    /// </summary>
    /// <remarks>
    /// Cán bộ có thể thu hồi lại một quyết định trợ cấp do sai sót trong quá trình nhập liệu ban đầu. Hệ thống chỉ cho phép hủy bỏ đối với những đợt chi trả chưa được hoàn thành.
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
    [HttpPut("{id}/cancel")]
    [Authorize(Policy = Permissions.Payments.Cancel)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] string reason)
    {
        var result = await _sender.Send(new CancelPaymentCommand(id, reason));
        return result.IsSuccess 
            ? Ok(ApiResult<bool>.SuccessResult(true, "Payment cancelled successfully."))
            : BadRequest(ApiResult<bool>.FailureResult(result.Error.Message));
    }

    /// <summary>
    /// Truy xuất toàn bộ thông tin chi tiết của một đợt chi trả cụ thể trên hệ thống.
    /// </summary>
    /// <remarks>
    /// Giao diện này cung cấp góc nhìn toàn diện về lịch sử, trạng thái và đối tượng thụ hưởng của một giao dịch. Cán bộ có thể dùng để đối soát dữ liệu với thực tế.
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
    [Authorize(Policy = Permissions.Payments.View)]
    [ProducesResponseType(typeof(ApiResult<PaymentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sender.Send(new GetPaymentByIdQuery(id));
        return result.IsSuccess 
            ? Ok(ApiResult<PaymentDto>.SuccessResult(result.Value))
            : NotFound(ApiResult<PaymentDto>.FailureResult(result.Error.Message));
    }

    /// <summary>
    /// Tìm kiếm và lọc danh sách các đợt chi trả dựa trên nhiều tiêu chí quản lý khác nhau.
    /// </summary>
    /// <remarks>
    /// Hỗ trợ cán bộ tìm kiếm nhanh các giao dịch bằng cách kết hợp nhiều điều kiện như thời gian, trạng thái hoặc số tiền. Kết quả trả về được phân trang để tối ưu hiệu suất hiển thị.
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
    [Authorize(Policy = Permissions.Payments.View)]
    [ProducesResponseType(typeof(ApiResult<AnSinhSo.Contracts.Common.PagedResult<PaymentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] SearchPaymentRequestDto request)
    {
        var result = await _sender.Send(new SearchPaymentsQuery(request));
        return result.IsSuccess 
            ? Ok(ApiResult<AnSinhSo.Contracts.Common.PagedResult<PaymentDto>>.SuccessResult(result.Value))
            : BadRequest(ApiResult<AnSinhSo.Contracts.Common.PagedResult<PaymentDto>>.FailureResult(result.Error.Message));
    }

    /// <summary>
    /// Liệt kê toàn bộ lịch sử các lần nhận tiền trợ cấp của một công dân cụ thể.
    /// </summary>
    /// <remarks>
    /// Tính năng này giúp theo dõi quá trình thụ hưởng chính sách an sinh xã hội của một cá nhân theo thời gian. Cán bộ có thể tra cứu để giải đáp thắc mắc cho người dân khi cần thiết.
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
    [HttpGet("citizen/{citizenId}")]
    [Authorize(Policy = Permissions.Payments.View)]
    [ProducesResponseType(typeof(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCitizenPayments(Guid citizenId)
    {
        var result = await _sender.Send(new GetCitizenPaymentsQuery(citizenId));
        return result.IsSuccess 
            ? Ok(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>.SuccessResult(result.Value))
            : BadRequest(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>.FailureResult(result.Error.Message));
    }

    /// <summary>
    /// Liệt kê lịch sử nhận tiền trợ cấp theo diện chính sách của một hộ gia đình cụ thể.
    /// </summary>
    /// <remarks>
    /// Hệ thống tổng hợp các khoản tiền được cấp phát cho tập thể hộ gia đình dựa trên sổ hộ khẩu. Quá trình này giúp đánh giá tổng mức hỗ trợ mà một gia đình đã nhận được.
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
    [HttpGet("household/{householdId}")]
    [Authorize(Policy = Permissions.Payments.View)]
    [ProducesResponseType(typeof(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHouseholdPayments(Guid householdId)
    {
        var result = await _sender.Send(new GetHouseholdPaymentsQuery(householdId));
        return result.IsSuccess 
            ? Ok(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>.SuccessResult(result.Value))
            : BadRequest(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>.FailureResult(result.Error.Message));
    }

    /// <summary>
    /// Hiển thị danh sách các đợt chi trả đang trong trạng thái chờ được xử lý hoặc chờ duyệt.
    /// </summary>
    /// <remarks>
    /// Chức năng này cung cấp danh sách công việc cần giải quyết cho cán bộ xử lý hoặc người quản lý phê duyệt. Việc theo dõi sát sao giúp tiền trợ cấp đến tay người dân đúng hạn.
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
    [HttpGet("pending")]
    [Authorize(Policy = Permissions.Payments.View)]
    [ProducesResponseType(typeof(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendingPayments()
    {
        var result = await _sender.Send(new GetPendingPaymentsQuery());
        return result.IsSuccess 
            ? Ok(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>.SuccessResult(result.Value))
            : BadRequest(ApiResult<System.Collections.Generic.IReadOnlyList<PaymentDto>>.FailureResult(result.Error.Message));
    }
}
