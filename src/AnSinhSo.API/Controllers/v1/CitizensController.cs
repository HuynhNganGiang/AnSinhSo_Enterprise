using System;
using System.Threading.Tasks;
using AnSinhSo.Application.Citizens.Commands.ActivateCitizen;
using AnSinhSo.Application.Citizens.Commands.CreateCitizen;
using AnSinhSo.Application.Citizens.Commands.DeactivateCitizen;
using AnSinhSo.Application.Citizens.Commands.UpdateCitizen;
using AnSinhSo.Application.Citizens.Queries.GetCitizenById;
using AnSinhSo.Application.Citizens.Queries.GetCitizens;
using AnSinhSo.Application.Citizens.Queries.SearchCitizen;
using AnSinhSo.Application.Citizens.Queries.SearchCitizenByIdentityNumber;
using AnSinhSo.Contracts.Citizens;
using AnSinhSo.Contracts.Common;
using AnSinhSo.Domain.Constants;
using AnSinhSo.Shared.Responses;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/citizens")]
[ApiController]
[Authorize]
public class CitizensController : ApiControllerBase
{
    private readonly ISender _sender;

    public CitizensController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Truy vấn và liệt kê danh sách thông tin cơ bản của các cư dân sinh sống trên địa bàn.
    /// </summary>
    /// <remarks>
    /// Đây là tính năng tra cứu danh bạ dân cư cốt lõi của toàn bộ hệ thống quản lý. Danh sách hỗ trợ phân trang để đảm bảo máy chủ không bị quá tải khi số lượng dân cư quá lớn.
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
    [Authorize(Policy = Permissions.Citizens.View)]
    [ProducesResponseType(typeof(ApiResult<PagedResult<CitizenDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCitizens([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sort = null)
    {
        var query = new GetCitizensQuery(page, pageSize, sort);
        var result = await _sender.Send(query);
        return result.IsSuccess ? Ok(ApiResult<PagedResult<CitizenDto>>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    /// <summary>
    /// Tìm kiếm thông tin cá nhân thông qua các tiêu chí như họ tên, số căn cước, hoặc ngày sinh.
    /// </summary>
    /// <remarks>
    /// Tính năng tra cứu nâng cao giúp cán bộ xã nhanh chóng xác định đúng người dân thông qua bộ lọc thông minh. Có thể kết hợp nhiều điều kiện tìm kiếm cùng một lúc để thu hẹp kết quả.
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
    [HttpGet("search")]
    [Authorize(Policy = Permissions.Citizens.View)]
    [ProducesResponseType(typeof(ApiResult<PagedResult<CitizenDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] SearchCitizenRequest request)
    {
        var query = new SearchCitizenQuery(request.IdentityNumber, request.Phone, request.Keyword, request.Page, request.PageSize, request.Sort);
        var result = await _sender.Send(query);
        return result.IsSuccess ? Ok(ApiResult<PagedResult<CitizenDto>>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    /// <summary>
    /// Truy xuất toàn bộ hồ sơ cá nhân của một người dân, bao gồm cả lịch sử nhận trợ cấp.
    /// </summary>
    /// <remarks>
    /// Trình bày một bức tranh toàn cảnh về nhân thân, hoàn cảnh và các chính sách hỗ trợ mà người dân đang thụ hưởng. Đây là nguồn dữ liệu quan trọng để đưa ra các quyết định phê duyệt hành chính.
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
    [Authorize(Policy = Permissions.Citizens.View)]
    [ProducesResponseType(typeof(ApiResult<CitizenDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetCitizenByIdQuery(id);
        var result = await _sender.Send(query);
        return result.IsSuccess ? Ok(ApiResult<CitizenDetailDto>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    /// <summary>
    /// Truy vấn nhanh thông tin hồ sơ của một người dân dựa trên mã số cá nhân duy nhất.
    /// </summary>
    /// <remarks>
    /// Cung cấp cách thức truy cập hồ sơ chính xác tuyệt đối thông qua số chứng minh hoặc thẻ căn cước. Giúp giảm thiểu rủi ro nhầm lẫn dữ liệu giữa những người trùng họ tên.
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
    [HttpGet("identity/{identityNumber}")]
    [Authorize(Policy = Permissions.Citizens.View)]
    [ProducesResponseType(typeof(ApiResult<CitizenDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdentityNumber(string identityNumber)
    {
        var query = new SearchCitizenByIdentityNumberQuery(identityNumber);
        var result = await _sender.Send(query);
        return result.IsSuccess ? Ok(ApiResult<CitizenDetailDto>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    /// <summary>
    /// Thêm mới hồ sơ nhân thân của một người vào cơ sở dữ liệu quản lý dân cư trung tâm.
    /// </summary>
    /// <remarks>
    /// Chức năng này dùng để số hóa và lưu trữ thông tin của công dân vào nền tảng điện tử của nhà nước. Thông tin ban đầu cần đảm bảo tính trung thực và khớp với giấy tờ tùy thân.
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
    [Authorize(Policy = Permissions.Citizens.Create)]
    [ProducesResponseType(typeof(ApiResult<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCitizenRequest request)
    {
        var command = new CreateCitizenCommand(request.CitizenNumber, request.FullName, request.BirthDate, request.Gender, request.PhoneNumber, request.Address, request.Email);
        var result = await _sender.Send(command);

        if (result.IsFailure) return HandleFailure(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, ApiResult<Guid>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Sửa đổi và bổ sung các thông tin cá nhân trong hồ sơ lưu trữ của một người cụ thể.
    /// </summary>
    /// <remarks>
    /// Cho phép cán bộ điều chỉnh dữ liệu bị sai sót hoặc cập nhật các thay đổi mới về nhân thân của công dân. Mọi thay đổi đều được ghi lại lịch sử để truy vết khi có khiếu nại.
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
    [Authorize(Policy = Permissions.Citizens.Update)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCitizenRequest request)
    {
        var command = new UpdateCitizenCommand(id, request.FullName, request.BirthDate, request.Gender, request.PhoneNumber, request.Address, request.Email);
        var result = await _sender.Send(command);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(ApiResult<bool>.SuccessResult(true));
    }

    /// <summary>
    /// Đưa hồ sơ cá nhân vào trạng thái hoạt động chính thức để sử dụng trong các quy trình.
    /// </summary>
    /// <remarks>
    /// Thao tác này xác nhận tính hợp lệ của hồ sơ sau khi đã đối chiếu với cơ sở dữ liệu quốc gia. Các hồ sơ hoạt động mới được phép tạo lập yêu cầu trợ cấp an sinh.
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
    [HttpPatch("{id:guid}/activate")]
    [Authorize(Policy = Permissions.Citizens.Activate)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid id)
    {
        var command = new ActivateCitizenCommand(id);
        var result = await _sender.Send(command);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(ApiResult<bool>.SuccessResult(true));
    }

    /// <summary>
    /// Đưa hồ sơ cá nhân vào trạng thái ngưng sử dụng do chuyển khẩu hoặc từ trần.
    /// </summary>
    /// <remarks>
    /// Đánh dấu hồ sơ không còn giá trị sử dụng nhằm tránh việc trục lợi các chính sách hỗ trợ của nhà nước. Dữ liệu hồ sơ vẫn được giữ nguyên để đối soát báo cáo cũ.
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
    [HttpPatch("{id:guid}/deactivate")]
    [Authorize(Policy = Permissions.Citizens.Deactivate)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var command = new DeactivateCitizenCommand(id);
        var result = await _sender.Send(command);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(ApiResult<bool>.SuccessResult(true));
    }

    /// <summary>
    /// Ghi nhận lại tọa độ định vị hiện tại của người dân trên bản đồ số của khu vực.
    /// </summary>
    /// <remarks>
    /// Hỗ trợ thu thập thông tin vị trí địa lý thực tế để quản lý rủi ro thiên tai và dịch tễ học. Dữ liệu này trực tiếp phục vụ cho chức năng hiển thị bản đồ tổng hợp.
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
    [HttpPatch("{id:guid}/location")]
    [Authorize(Policy = Permissions.Map.UpdateLocation)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLocation(Guid id, [FromBody] UpdateLocationRequest request)
    {
        var command = new AnSinhSo.Application.Citizens.Commands.UpdateLocation.UpdateCitizenLocationCommand(id, request.Latitude, request.Longitude);
        var result = await _sender.Send(command);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(ApiResult<bool>.SuccessResult(true));
    }
}
