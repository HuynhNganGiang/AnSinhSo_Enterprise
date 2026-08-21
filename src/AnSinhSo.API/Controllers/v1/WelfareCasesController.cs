using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AnSinhSo.Shared.Responses;
using Asp.Versioning;
using MediatR;
using AnSinhSo.Application.WelfareCases.Commands.CancelWelfareCase;
using AnSinhSo.Application.WelfareCases.Commands.CloseWelfareCase;
using AnSinhSo.Application.WelfareCases.Commands.CreateWelfareCase;
using AnSinhSo.Application.WelfareCases.Commands.MakeWelfareCaseDecision;
using AnSinhSo.Application.WelfareCases.Commands.SubmitWelfareCase;
using AnSinhSo.Application.WelfareCases.Commands.UpdateWelfareCase;
using AnSinhSo.Application.WelfareCases.DTOs;
using AnSinhSo.Application.WelfareCases.Queries.GetCitizenWelfareCases;
using AnSinhSo.Application.WelfareCases.Queries.GetWelfareCaseById;
using AnSinhSo.Application.WelfareCases.Queries.SearchWelfareCases;
using AnSinhSo.Contracts.Common;
using AnSinhSo.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/welfarecases")]
[ApiController]
public sealed class WelfareCasesController : ApiControllerBase
{
    private readonly ISender _sender;

    public WelfareCasesController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Tiếp nhận và khởi tạo một hồ sơ đề nghị hưởng chế độ hỗ trợ xã hội cho người dân.
    /// </summary>
    /// <remarks>
    /// Cán bộ cơ sở sử dụng tính năng này để nhập thông tin đơn từ và tải lên các tài liệu chứng minh hoàn cảnh. Hồ sơ mới tạo sẽ bước vào luồng xử lý hành chính nhiều cấp để xem xét.
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
    [Authorize(Policy = Permissions.WelfareCases.Create)]
    [ProducesResponseType(typeof(ApiResult<Guid>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateWelfareCase([FromBody] CreateWelfareCaseCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetWelfareCaseById), new { id = result.Value }, ApiResult<Guid>.SuccessResult(result.Value));
        }
        return HandleFailure(result);
    }

    /// <summary>
    /// Truy xuất toàn bộ nội dung tài liệu và lịch sử các bước xét duyệt của một bộ hồ sơ cụ thể.
    /// </summary>
    /// <remarks>
    /// Giao diện này cho phép các bên liên quan kiểm tra độ chính xác của thông tin và xem tiến trình thụ lý hồ sơ đến đâu. Mọi quyết định phê duyệt đều được hệ thống lưu vết rõ ràng tại đây.
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
    [Authorize(Policy = Permissions.WelfareCases.View)]
    [ProducesResponseType(typeof(ApiResult<WelfareCaseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWelfareCaseById(Guid id)
    {
        var result = await _sender.Send(new GetWelfareCaseByIdQuery(id));
        return result.IsSuccess ? Ok(ApiResult<WelfareCaseDto>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    /// <summary>
    /// Tìm kiếm và truy vấn các bộ hồ sơ đề nghị trợ cấp dựa trên các điều kiện lọc do cán bộ cung cấp.
    /// </summary>
    /// <remarks>
    /// Bộ máy tìm kiếm giúp cán bộ thụ lý quản lý danh sách công việc hàng ngày một cách hiệu quả thông qua việc lọc trạng thái. Cán bộ có thể dễ dàng tìm ra các hồ sơ quá hạn cần giải quyết gấp.
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
    [Authorize(Policy = Permissions.WelfareCases.View)]
    [ProducesResponseType(typeof(ApiResult<PagedResult<WelfareCaseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchWelfareCases(
        [FromQuery] string? keyword,
        [FromQuery] Guid? programId,
        [FromQuery] int? statusId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sort = null)
    {
        var query = new SearchWelfareCasesQuery(keyword, programId, statusId, page, pageSize, sort);
        var result = await _sender.Send(query);
        return result.IsSuccess ? Ok(ApiResult<PagedResult<WelfareCaseDto>>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    /// <summary>
    /// Liệt kê toàn bộ lịch sử các hồ sơ đề nghị hưởng chế độ mà một cá nhân từng nộp trước đây.
    /// </summary>
    /// <remarks>
    /// Tính năng này giúp đối soát quá trình tham gia thụ hưởng chính sách của người dân để ngăn ngừa việc nộp trùng lặp thủ tục. Toàn bộ các hồ sơ từ trước đến nay đều được hệ thống bảo lưu đầy đủ.
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
    [HttpGet("citizen/{citizenId:guid}")]
    [Authorize(Policy = Permissions.WelfareCases.View)]
    [ProducesResponseType(typeof(ApiResult<IReadOnlyList<WelfareCaseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCitizenWelfareCases(Guid citizenId)
    {
        var result = await _sender.Send(new GetCitizenWelfareCasesQuery(citizenId));
        return result.IsSuccess ? Ok(ApiResult<IReadOnlyList<WelfareCaseDto>>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    /// <summary>
    /// Bổ sung hoặc sửa đổi các thông tin chi tiết chưa chính xác bên trong một bộ hồ sơ đang xét duyệt.
    /// </summary>
    /// <remarks>
    /// Cán bộ có thể yêu cầu người dân nộp bổ sung giấy tờ và sử dụng chức năng này để cập nhật vào hệ thống. Việc sửa đổi chỉ được phép thực hiện trước khi có quyết định phê duyệt chính thức.
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
    [Authorize(Policy = Permissions.WelfareCases.Update)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateWelfareCase(Guid id, [FromBody] UpdateWelfareCaseCommand command)
    {
        if (id != command.WelfareCaseId) return BadRequest();
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(ApiResult.SuccessResult()) : HandleFailure(result);
    }

    /// <summary>
    /// Chuyển giao bộ hồ sơ lên cơ quan quản lý cấp cao hơn để tiến hành các bước thẩm định chuyên môn.
    /// </summary>
    /// <remarks>
    /// Đây là một thao tác chuyển trạng thái quan trọng trong luồng xử lý văn bản hành chính điện tử của hệ thống. Người nhận ở cấp trên sẽ nhận được cảnh báo có công việc mới cần phải giải quyết ngay.
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
    [HttpPost("{id:guid}/submit")]
    [Authorize(Policy = Permissions.WelfareCases.Update)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> SubmitWelfareCase(Guid id, [FromBody] SubmitWelfareCaseCommand command)
    {
        if (id != command.WelfareCaseId) return BadRequest();
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(ApiResult.SuccessResult()) : HandleFailure(result);
    }

    /// <summary>
    /// Phê duyệt hoặc từ chối chấp thuận một hồ sơ đề nghị cấp vốn kèm theo lý do cụ thể từ cán bộ.
    /// </summary>
    /// <remarks>
    /// Hành động này mang tính chất pháp lý quyết định việc người dân có được nhận tiền hỗ trợ từ ngân sách hay không. Nếu bị từ chối, cán bộ bắt buộc phải giải trình rõ căn cứ pháp luật để người dân hiểu.
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
    [HttpPatch("{id:guid}/decision")]
    [Authorize(Policy = Permissions.WelfareCases.Decide)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> MakeWelfareCaseDecision(Guid id, [FromBody] MakeWelfareCaseDecisionCommand command)
    {
        if (id != command.WelfareCaseId) return BadRequest();
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(ApiResult.SuccessResult()) : HandleFailure(result);
    }

    /// <summary>
    /// Đình chỉ và thu hồi lại một bộ hồ sơ do phát hiện có dấu hiệu sai lệch thông tin trong quá trình xử lý.
    /// </summary>
    /// <remarks>
    /// Chức năng này dùng để xử lý các trường hợp người dân rút đơn kiện hoặc cán bộ phát hiện hồ sơ bị làm giả tài liệu. Hồ sơ bị hủy sẽ vĩnh viễn không thể khôi phục lại trạng thái bình thường.
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
    [HttpPost("{id:guid}/cancel")]
    [Authorize(Policy = Permissions.WelfareCases.Cancel)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CancelWelfareCase(Guid id, [FromBody] CancelWelfareCaseCommand command)
    {
        if (id != command.WelfareCaseId) return BadRequest();
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(ApiResult.SuccessResult()) : HandleFailure(result);
    }

    /// <summary>
    /// Kết thúc hoàn toàn quy trình thụ lý của một bộ hồ sơ và lưu trữ vĩnh viễn vào kho dữ liệu điện tử.
    /// </summary>
    /// <remarks>
    /// Thao tác này đánh dấu sự khép lại của toàn bộ quy trình hành chính sau khi người dân đã nhận đủ tiền hỗ trợ. Hồ sơ đã đóng sẽ được chuyển sang dạng chỉ đọc để phục vụ thanh tra sau này.
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
    [HttpPost("{id:guid}/close")]
    [Authorize(Policy = Permissions.WelfareCases.Close)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CloseWelfareCase(Guid id, [FromBody] CloseWelfareCaseCommand command)
    {
        if (id != command.WelfareCaseId) return BadRequest();
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(ApiResult.SuccessResult()) : HandleFailure(result);
    }
}
