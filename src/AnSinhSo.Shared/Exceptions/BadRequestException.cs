using System;

namespace AnSinhSo.Shared.Exceptions
{
    /// <summary>
    /// Ngoại lệ ném ra khi yêu cầu không hợp lệ (HTTP 400).
    /// </summary>
    public class BadRequestException : AppException
    {
        /// <summary>
        /// Mã trạng thái HTTP 400 BadRequest.
        /// </summary>
        public override int StatusCode => 400;

        /// <summary>
        /// Khởi tạo BadRequestException.
        /// </summary>
        public BadRequestException() : base("Yêu cầu không hợp lệ.") { }

        /// <summary>
        /// Khởi tạo BadRequestException với thông điệp tùy chỉnh.
        /// </summary>
        public BadRequestException(string message) : base(message) { }
    }
}
