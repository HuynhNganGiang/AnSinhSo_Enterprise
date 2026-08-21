using System;
using System.Threading.Tasks;
using AnSinhSo.Application.Households.Commands.ActivateHousehold;
using AnSinhSo.Application.Households.Commands.AddHouseholdMember;
using AnSinhSo.Application.Households.Commands.ChangeHouseholdHead;
using AnSinhSo.Application.Households.Commands.CreateHousehold;
using AnSinhSo.Application.Households.Commands.DeactivateHousehold;
using AnSinhSo.Application.Households.Commands.RemoveHouseholdMember;
using AnSinhSo.Application.Households.Commands.UpdateHouseholdAddress;
using AnSinhSo.Application.Households.Queries.GetHouseholdById;
using AnSinhSo.Application.Households.Queries.SearchHouseholds;
using AnSinhSo.Application.Households.DTOs;
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
[Route("api/v{version:apiVersion}/households")]
[ApiController]
[Authorize]
[Tags("Households")]
public class HouseholdsController : ApiControllerBase
{
    private readonly ISender _sender;

    public HouseholdsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Tìm kiếm sổ hộ khẩu của các gia đình dựa trên thông tin chủ hộ hoặc địa chỉ sinh sống.
    /// </summary>
    /// <remarks>
    /// Cung cấp công cụ tra cứu mạnh mẽ giúp định vị nhanh hồ sơ quản lý hành chính của một gia đình. Chức năng hỗ trợ đối chiếu thông tin tập thể thay vì từng cá nhân đơn lẻ.
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
    [Authorize(Policy = Permissions.Households.Read)]
    [ProducesResponseType(typeof(ApiResult<PagedResult<HouseholdSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] SearchHouseholdsQuery query)
    {
        var result = await _sender.Send(query);
        return result.IsSuccess ? Ok(ApiResult<PagedResult<HouseholdSummaryDto>>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    /// <summary>
    /// Truy xuất toàn bộ dữ liệu cấu trúc của một hộ gia đình bao gồm danh sách các thành viên.
    /// </summary>
    /// <remarks>
    /// Hiển thị chi tiết mối quan hệ nhân thân giữa các cá nhân sinh sống dưới cùng một mái nhà. Dữ liệu này dùng để xét duyệt các chính sách an sinh áp dụng theo quy mô tập thể.
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
    [Authorize(Policy = Permissions.Households.Read)]
    [ProducesResponseType(typeof(ApiResult<HouseholdDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetHouseholdByIdQuery(id);
        var result = await _sender.Send(query);
        return result.IsSuccess ? Ok(ApiResult<HouseholdDto>.SuccessResult(result.Value)) : HandleFailure(result);
    }

    /// <summary>
    /// Lập mới một hồ sơ quản lý hộ gia đình trên hệ thống dữ liệu điện tử tập trung.
    /// </summary>
    /// <remarks>
    /// Chức năng số hóa quy trình cấp sổ hộ khẩu giấy thông qua việc lưu trữ hồ sơ gia đình trực tuyến. Cán bộ cần chỉ định người đứng tên chủ hộ trong bước khởi tạo này.
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
    [Authorize(Policy = Permissions.Households.Create)]
    [ProducesResponseType(typeof(ApiResult<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateHouseholdCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsFailure) return HandleFailure(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, ApiResult<Guid>.SuccessResult(result.Value));
    }

    /// <summary>
    /// Đưa hồ sơ gia đình vào trạng thái hoạt động để bắt đầu xét duyệt các thủ tục hành chính.
    /// </summary>
    /// <remarks>
    /// Xác nhận hồ sơ đã hoàn thiện đầy đủ pháp lý và sẵn sàng tiếp nhận các quyền lợi xã hội. Đây là bước kiểm duyệt bắt buộc sau khi tiến hành nhập liệu thông tin ban đầu.
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
    [Authorize(Policy = Permissions.Households.Update)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid id)
    {
        var command = new ActivateHouseholdCommand(id);
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(ApiResult<bool>.SuccessResult(true)) : HandleFailure(result);
    }

    /// <summary>
    /// Khóa hồ sơ gia đình trong trường hợp giải thể hộ hoặc chuyển toàn bộ đi nơi khác.
    /// </summary>
    /// <remarks>
    /// Thao tác này giúp ngừng phân bổ các gói hỗ trợ tập thể cho các gia đình không còn sinh sống tại địa bàn. Việc này đảm bảo tính minh bạch và tránh thất thoát ngân sách địa phương.
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
    [Authorize(Policy = Permissions.Households.Update)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var command = new DeactivateHouseholdCommand(id);
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(ApiResult<bool>.SuccessResult(true)) : HandleFailure(result);
    }

    /// <summary>
    /// Chuyển đổi thông tin địa chỉ cư trú của toàn bộ tập thể gia đình sang một nơi ở mới.
    /// </summary>
    /// <remarks>
    /// Giao diện này cho phép cán bộ cập nhật biến động địa chỉ của hộ khẩu đồng loạt. Việc đổi địa chỉ có thể ảnh hưởng đến nơi nhận tiền trợ cấp hàng tháng của gia đình.
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
    [HttpPut("{id:guid}/address")]
    [Authorize(Policy = Permissions.Households.Update)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAddress(Guid id, [FromBody] UpdateHouseholdAddressCommand command)
    {
        var request = command with { HouseholdId = id };
        var result = await _sender.Send(request);
        return result.IsSuccess ? Ok(ApiResult<bool>.SuccessResult(true)) : HandleFailure(result);
    }

    /// <summary>
    /// Bổ sung thêm một cá nhân mới vào danh sách nhân khẩu trực thuộc của một hộ cụ thể.
    /// </summary>
    /// <remarks>
    /// Tính năng dùng để giải quyết các thủ tục nhập khẩu như khai sinh, kết hôn hoặc chuyển khẩu đến. Cá nhân được thêm vào sẽ được hưởng các quyền lợi chung của toàn hộ gia đình.
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
    [HttpPost("{id:guid}/members")]
    [Authorize(Policy = Permissions.Households.Update)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddMember(Guid id, [FromBody] AddHouseholdMemberCommand command)
    {
        var request = command with { HouseholdId = id };
        var result = await _sender.Send(request);
        return result.IsSuccess ? Ok(ApiResult<bool>.SuccessResult(true)) : HandleFailure(result);
    }

    /// <summary>
    /// Loại bỏ thông tin một cá nhân ra khỏi danh sách nhân khẩu trực thuộc do tách hộ hoặc từ trần.
    /// </summary>
    /// <remarks>
    /// Hỗ trợ cắt khẩu cho người dân không còn chung sống với gia đình vì nhiều lý do khác nhau. Thao tác này sẽ tự động thay đổi cấu trúc dân số tính toán của địa phương.
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
    [HttpDelete("{id:guid}/members/{memberId:guid}")]
    [Authorize(Policy = Permissions.Households.Update)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveMember(Guid id, Guid memberId)
    {
        var command = new RemoveHouseholdMemberCommand(id, memberId);
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(ApiResult<bool>.SuccessResult(true)) : HandleFailure(result);
    }

    /// <summary>
    /// Chỉ định một thành viên khác trong gia đình lên làm người đại diện mới thay cho người cũ.
    /// </summary>
    /// <remarks>
    /// Thực hiện việc chuyển đổi quyền đại diện pháp luật của gia đình sang một cá nhân đủ năng lực hành vi. Yêu cầu thành viên được chọn phải có đủ tiêu chuẩn theo quy định hiện hành.
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
    [HttpPut("{id:guid}/head")]
    [Authorize(Policy = Permissions.Households.Update)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeHead(Guid id, [FromBody] ChangeHouseholdHeadCommand command)
    {
        var request = command with { HouseholdId = id };
        var result = await _sender.Send(request);
        return result.IsSuccess ? Ok(ApiResult<bool>.SuccessResult(true)) : HandleFailure(result);
    }

    /// <summary>
    /// Cập nhật lại thông tin tọa độ sinh sống của hộ gia đình lên nền tảng bản đồ số hóa.
    /// </summary>
    /// <remarks>
    /// Chức năng này giúp số hóa không gian cư trú để dễ dàng quản lý việc phân bổ hạ tầng và cứu trợ khu vực. Dữ liệu tọa độ góp phần xây dựng hệ sinh thái đô thị thông minh.
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
        var command = new AnSinhSo.Application.Households.Commands.UpdateLocation.UpdateHouseholdLocationCommand(id, request.Latitude, request.Longitude);
        var result = await _sender.Send(command);

        if (result.IsFailure) return HandleFailure(result);

        return Ok(ApiResult<bool>.SuccessResult(true));
    }
}
