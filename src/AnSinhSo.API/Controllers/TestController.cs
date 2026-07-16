using AnSinhSo.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers
{
    /// <summary>
    /// Controller thử nghiệm để kiểm tra và xác minh hoạt động của các Middleware và chuẩn phản hồi.
    /// </summary>
    public class TestController : BaseController
    {
        /// <summary>
        /// Lấy phản hồi thành công và kiểm tra ResponseWrapperMiddleware tự động bọc payload.
        /// </summary>
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
        /// Lấy phản hồi lỗi để kiểm tra GlobalExceptionMiddleware bắt và bọc lỗi.
        /// </summary>
        [HttpGet("error")]
        public IActionResult GetError()
        {
            throw new BadRequestException("Đây là lỗi giả lập để kiểm tra hoạt động của GlobalExceptionMiddleware.");
        }
    }
}
