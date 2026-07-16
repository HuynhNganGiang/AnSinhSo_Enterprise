using System;

namespace AnSinhSo.Shared.Exceptions
{
    /// <summary>
    /// Ngoại lệ ném ra khi người dùng chưa xác thực (HTTP 401).
    /// </summary>
    public class UnauthorizedException : AppException
    {
        /// <summary>
        /// Mã trạng thái HTTP 401 Unauthorized.
        /// </summary>
        public override int StatusCode => 401;

        /// <summary>
        /// Khởi tạo UnauthorizedException.
        /// </summary>
        public UnauthorizedException() : base("Chưa xác thực danh tính.") { }

        /// <summary>
        /// Khởi tạo UnauthorizedException với thông điệp tùy chỉnh.
        /// </summary>
        public UnauthorizedException(string message) : base(message) { }
    }
}
