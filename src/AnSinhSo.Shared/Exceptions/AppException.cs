using System;

namespace AnSinhSo.Shared.Exceptions
{
    /// <summary>
    /// Ngoại lệ cơ sở của ứng dụng AnSinhSo.
    /// </summary>
    public class AppException : Exception
    {
        /// <summary>
        /// Mã trạng thái HTTP mong muốn trả về.
        /// </summary>
        public virtual int StatusCode => 500;

        /// <summary>
        /// Khởi tạo một thực thể mới của AppException.
        /// </summary>
        public AppException() : base() { }

        /// <summary>
        /// Khởi tạo một thực thể mới của AppException với thông điệp lỗi.
        /// </summary>
        public AppException(string message) : base(message) { }

        /// <summary>
        /// Khởi tạo một thực thể mới của AppException với thông điệp lỗi và ngoại lệ lồng bên trong.
        /// </summary>
        public AppException(string message, Exception innerException) : base(message, innerException) { }
    }
}
