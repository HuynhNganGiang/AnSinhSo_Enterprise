using System.Threading.Tasks;
using AnSinhSo.Application.WelfarePrograms.Queries.GetWelfarePrograms;
using AnSinhSo.Application.WelfarePrograms.DTOs;
using AnSinhSo.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AnSinhSo.Shared.Responses;
using Asp.Versioning;
using MediatR;

namespace AnSinhSo.API.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/welfareprograms")]
[ApiController]
public sealed class WelfareProgramsController : ApiControllerBase
{
    private readonly ISender _sender;

    public WelfareProgramsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Liệt kê các chương trình hỗ trợ xã hội và dự án từ thiện đang được triển khai trên toàn địa bàn.
    /// </summary>
    /// <remarks>
    /// Chức năng cung cấp danh mục các loại hình chính sách mà người dân có thể nộp đơn xin thụ hưởng. Cán bộ dùng danh sách này để phân loại nguồn vốn và đối tượng áp dụng cho các hồ sơ cấp phát.
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
    [Authorize(Policy = Permissions.WelfarePrograms.View)]
    [ProducesResponseType(typeof(ApiResult<IReadOnlyList<WelfareProgramDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWelfarePrograms()
    {
        var result = await _sender.Send(new GetWelfareProgramsQuery());
        return result.IsSuccess ? Ok(ApiResult<IReadOnlyList<WelfareProgramDto>>.SuccessResult(result.Value)) : HandleFailure(result);
    }
}
