using System;
using System.Threading.Tasks;
using AnSinhSo.Application.AI.Commands.AnalyzeCitizen;
using AnSinhSo.Application.AI.Commands.AnalyzeHousehold;
using AnSinhSo.Application.AI.Commands.ScanAll;
using AnSinhSo.Application.AI.Commands.UpdateRecommendationStatus;
using AnSinhSo.Application.AI.Queries.GetAiDashboardSummary;
using AnSinhSo.Application.AI.Queries.GetAiRecommendations;
using AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;
using AnSinhSo.Domain.Constants;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers.V1;

[Route("api/v1/ai")]
[ApiController]
public class AiController : ControllerBase
{
    private readonly ISender _sender;

    public AiController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Sử dụng trí tuệ nhân tạo thông minh để phân tích và đánh giá hoàn cảnh thực tế của hộ gia đình.
    /// </summary>
    /// <remarks>
    /// Hệ thống thuật toán nâng cao sẽ đối chiếu thông tin gia đình với các tiêu chí an sinh phức tạp. Kết quả trả về không chỉ là điểm số mà còn kèm theo lời giải thích chi tiết về lý do đánh giá.
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
    [HttpPost("analyze-household/{id}")]
    [Authorize(Policy = Permissions.AI.Analyze)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AnalyzeHousehold(Guid id)
    {
        var command = new AnalyzeHouseholdCommand(id);
        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(Result.Success());
    }

    /// <summary>
    /// Ứng dụng trí tuệ nhân tạo để chấm điểm và đánh giá mức độ khó khăn của từng cá nhân.
    /// </summary>
    /// <remarks>
    /// Chương trình sẽ tự động trích xuất các rủi ro xã hội từ hồ sơ người dân để đưa ra kết luận mức độ cần hỗ trợ. Các lý luận của máy tính được trình bày bằng ngôn ngữ tự nhiên để cán bộ dễ dàng hiểu được.
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
    [HttpPost("analyze-citizen/{id}")]
    [Authorize(Policy = Permissions.AI.Analyze)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AnalyzeCitizen(Guid id)
    {
        var command = new AnalyzeCitizenCommand(id);
        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(Result.Success());
    }

    /// <summary>
    /// Quét tổng thể dữ liệu toàn hệ thống để tự động tạo ra các đề xuất trợ cấp phù hợp nhất.
    /// </summary>
    /// <remarks>
    /// Đây là một quy trình ngầm phân tích hàng vạn hồ sơ dân cư trong hệ thống nhằm phát hiện các trường hợp bị bỏ sót. Nếu tìm thấy trường hợp phù hợp, máy tính sẽ tự lập danh sách đề xuất cấp phát trợ cấp.
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
    [HttpPost("scan-all")]
    [Authorize(Policy = Permissions.AI.Scan)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ScanAll()
    {
        var command = new ScanAllCommand();
        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(Result.Success());
    }

    /// <summary>
    /// Liệt kê các đề xuất trợ cấp do trí tuệ nhân tạo tự động đánh giá và gợi ý cho cán bộ.
    /// </summary>
    /// <remarks>
    /// Giao diện này hiển thị danh sách những người dân có khả năng cao đạt đủ điều kiện nhận hỗ trợ theo phán đoán của máy. Cán bộ có thể dựa vào danh sách này để chủ động tiếp cận và hướng dẫn làm thủ tục.
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
    [HttpGet("recommendations")]
    [Authorize(Policy = Permissions.AI.ViewRecommendations)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecommendations([FromQuery] AiTargetType? targetType, [FromQuery] AiRecommendationStatus? status)
    {
        var query = new GetAiRecommendationsQuery { TargetType = targetType, Status = status };
        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    /// <summary>
    /// Truy xuất các số liệu thống kê tổng quan về tình hình ứng dụng công nghệ trên địa bàn.
    /// </summary>
    /// <remarks>
    /// Tổng hợp dữ liệu thành các con số báo cáo trực quan giúp lãnh đạo nắm bắt được hiệu quả hoạt động của toàn hệ thống. Thông tin được cập nhật theo thời gian thực để phản ánh đúng thực tế nhất.
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
    [HttpGet("dashboard-summary")]
    [Authorize(Policy = Permissions.AI.Dashboard)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboardSummary()
    {
        var query = new GetAiDashboardSummaryQuery();
        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    /// <summary>
    /// Thay đổi trạng thái xử lý đối với một đề xuất trợ cấp do máy tính tính toán và khuyến nghị.
    /// </summary>
    /// <remarks>
    /// Cán bộ có thể chấp nhận hoặc từ chối các gợi ý từ hệ thống máy tính dựa trên xác minh thực tế. Phản hồi này giúp máy tính học hỏi và cải thiện độ chính xác trong tương lai.
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
    [HttpPatch("recommendations/{id}/status")]
    [Authorize(Policy = Permissions.AI.UpdateStatus)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateAiStatusRequest request)
    {
        var reviewedBy = User.Identity?.Name ?? "System";
        var command = new UpdateRecommendationStatusCommand(id, request.Status, reviewedBy);
        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            if (result.Error.Code.Contains("NotFound"))
            {
                return NotFound(result.Error);
            }
            return BadRequest(result.Error);
        }

        return Ok(Result.Success());
    }
}

public class UpdateAiStatusRequest
{
    public AiRecommendationStatus Status { get; set; }
}
