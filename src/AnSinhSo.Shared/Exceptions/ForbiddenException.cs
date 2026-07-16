using System;

namespace AnSinhSo.Shared.Exceptions
{
    /// <summary>
    /// Ngoại lệ ném ra khi người dùng không có quyền truy cập tài nguyên (HTTP 403).
    /// </summary>
    public class ForbiddenException : AppException
    {
        /// <summary>
        /// Mã trạng thái HTTP 403 Forbidden.
        /// </summary>
        public override int StatusCode => 403;

        /// <summary>
        /// Khởi tạo ForbiddenException.
        /// </summary>
        public ForbiddenException() : base("Không có quyền truy cập tài nguyên này.") { }

        /// <summary>
        /// Khởi tạo ForbiddenException với thông điệp tùy chỉnh.
        /// </summary>
        public ForbiddenException(string message) : base(message) { }
    }
}
