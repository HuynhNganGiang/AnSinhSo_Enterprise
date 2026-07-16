using System;

namespace AnSinhSo.Shared.Exceptions
{
    /// <summary>
    /// Ngoại lệ ném ra khi không tìm thấy tài nguyên (HTTP 404).
    /// </summary>
    public class NotFoundException : AppException
    {
        /// <summary>
        /// Mã trạng thái HTTP 404 NotFound.
        /// </summary>
        public override int StatusCode => 404;

        /// <summary>
        /// Khởi tạo NotFoundException.
        /// </summary>
        public NotFoundException() : base("Tài nguyên yêu cầu không được tìm thấy.") { }

        /// <summary>
        /// Khởi tạo NotFoundException với thông điệp tùy chỉnh.
        /// </summary>
        public NotFoundException(string message) : base(message) { }
    }
}
