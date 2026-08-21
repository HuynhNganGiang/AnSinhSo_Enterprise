using AnSinhSo.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers
{
    /// <summary>
    /// Điểm cuối thử nghiệm để kiểm tra hoạt động của các thành phần hệ thống và chuẩn định dạng kết quả.
    /// </summary>
    /// <remarks>
    /// Giao diện lập trình này hoàn toàn dùng cho mục đích kiểm thử nội bộ để đảm bảo hệ thống vận hành đúng luồng. Dữ liệu trả về luôn tuân thủ cấu trúc quy chuẩn chung của toàn dự án.
    /// </remarks>
    public class TestController : BaseController
    {
    /// <summary>
    /// Lấy phản hồi thành công mô phỏng để kiểm tra cơ chế tự động bọc dữ liệu của hệ thống.
    /// </summary>
    /// <remarks>
    /// Giao diện lập trình này trả về một đối tượng dữ liệu mẫu để hệ thống lõi tự động định dạng lại trước khi xuất ra. Mục đích là để đảm bảo mọi dữ liệu xuất ra đều có cấu trúc nhất quán.
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
        [HttpGet("success")]
        public IActionResult GetSuccess()
        {
            var testData = new
            {
                Name = "AnSinhSo Enterprise Foundation",
                Status = "Active",
                Sprint = 1
            };
            return Ok(testData); // Trả về object thuần, ResponseWrapperMiddleware sẽ tự động bọc thành ApiResult<T>
        }

    /// <summary>
    /// Lấy phản hồi lỗi mô phỏng để kiểm tra cơ chế xử lý ngoại lệ tự động của toàn cục hệ thống.
    /// </summary>
    /// <remarks>
    /// Giao diện lập trình này cố tình tạo ra một lỗi nhằm thử nghiệm khả năng bắt lỗi tự động của kiến trúc lõi. Nó giúp xác minh rằng các thông báo lỗi luôn được che giấu kỹ thuật và thân thiện với người dùng.
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
        [HttpGet("error")]
        public IActionResult GetError()
        {
            throw new BadRequestException("Đây là lỗi giả lập để kiểm tra hoạt động của GlobalExceptionMiddleware.");
        }
    }
}
