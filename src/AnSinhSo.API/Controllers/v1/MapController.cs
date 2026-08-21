using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Map.DTOs;
using AnSinhSo.Application.Map.Queries;
using AnSinhSo.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers.v1;

[Route("api/v1/map")]
[Authorize]
public class MapController : ApiControllerBase
{
    private readonly ISender _sender;

    public MapController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Trích xuất danh sách tọa độ của các đối tượng để phục vụ việc hiển thị trên bản đồ đồ họa.
    /// </summary>
    /// <remarks>
    /// Giao diện lập trình cung cấp tập hợp dữ liệu vị trí không gian để vẽ các điểm ghim trên bản đồ người dùng. Tập dữ liệu được tối ưu hóa để tải nhanh trên trình duyệt web.
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
    [Authorize(Policy = Permissions.Map.View)]
    [ProducesResponseType(typeof(List<MapMarkerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMapMarkers(
        [FromQuery] bool citizens = true,
        [FromQuery] bool households = true,
        [FromQuery] bool paymentPoints = true,
        [FromQuery] bool welfare = true,
        [FromQuery] string? keyword = null,
        [FromQuery] bool? poor = null,
        [FromQuery] bool? nearPoor = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMapMarkersQuery(citizens, households, paymentPoints, welfare, keyword, poor, nearPoor);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Tổng hợp các số liệu liên quan đến phân bố địa lý để vẽ các biểu đồ báo cáo trực quan.
    /// </summary>
    /// <remarks>
    /// Máy chủ sẽ tính toán mật độ dân cư và mật độ phân bổ điểm chi trả theo từng phân vùng không gian. Kết quả trả về giúp cán bộ quản lý có cái nhìn tổng quan về mức độ phủ sóng của chính sách.
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
    [Authorize(Policy = Permissions.Map.View)]
    [ProducesResponseType(typeof(MapStatisticsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMapStatistics(CancellationToken cancellationToken = default)
    {
        var query = new GetMapStatisticsQuery();
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Truy xuất tập hợp vị trí nơi ở của người dân để biểu diễn trên giao diện đồ họa bản đồ.
    /// </summary>
    /// <remarks>
    /// Tính năng này phục vụ cho việc quan sát mức độ tập trung dân số của các khu vực hành chính. Thông qua màu sắc và mật độ điểm ghim, cán bộ có thể nhận diện các khu dân cư đông đúc.
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
    [HttpGet("citizens")]
    [Authorize(Policy = Permissions.Map.View)]
    [ProducesResponseType(typeof(List<MapMarkerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCitizensMap(
        [FromQuery] string? keyword = null,
        [FromQuery] bool? poor = null,
        [FromQuery] bool? nearPoor = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMapMarkersQuery(true, false, false, false, keyword, poor, nearPoor);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Truy xuất dữ liệu tọa độ để biểu diễn mức độ phân tán của các gia đình trên bản đồ khu vực.
    /// </summary>
    /// <remarks>
    /// Hỗ trợ cán bộ địa chính quản lý quy hoạch và phân bổ dân cư một cách trực quan trên không gian hai chiều. Các cụm gia đình nghèo sẽ được đánh dấu rõ ràng để dễ dàng phân bổ nguồn lực.
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
    [HttpGet("households")]
    [Authorize(Policy = Permissions.Map.View)]
    [ProducesResponseType(typeof(List<MapMarkerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHouseholdsMap(
        [FromQuery] string? keyword = null,
        [FromQuery] bool? poor = null,
        [FromQuery] bool? nearPoor = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMapMarkersQuery(false, true, false, false, keyword, poor, nearPoor);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Hiển thị vị trí tọa độ của các địa điểm phát tiền trợ cấp đang hoạt động trên hệ thống bản đồ.
    /// </summary>
    /// <remarks>
    /// Chức năng này dùng để tối ưu hóa mạng lưới điểm phát tiền sao cho khoảng cách di chuyển của người dân là ngắn nhất. Nó trực tiếp cải thiện trải nghiệm nhận tiền hỗ trợ của cộng đồng.
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
    [HttpGet("payment-points")]
    [Authorize(Policy = Permissions.Map.View)]
    [ProducesResponseType(typeof(List<MapMarkerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentPointsMap(
        [FromQuery] string? keyword = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMapMarkersQuery(false, false, true, false, keyword, null, null);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Tổng hợp và hiển thị toàn bộ mạng lưới các đối tượng nhận hỗ trợ xã hội trên bản đồ số.
    /// </summary>
    /// <remarks>
    /// Bản đồ đa lớp cung cấp cái nhìn toàn diện về bức tranh phúc lợi xã hội của toàn bộ địa phương. Các cán bộ lãnh đạo có thể sử dụng biểu đồ này trong các cuộc họp lập kế hoạch chiến lược.
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
    [HttpGet("welfare")]
    [Authorize(Policy = Permissions.Map.View)]
    [ProducesResponseType(typeof(List<MapMarkerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWelfareMap(
        [FromQuery] string? keyword = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMapMarkersQuery(false, false, false, true, keyword, null, null);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }
}
