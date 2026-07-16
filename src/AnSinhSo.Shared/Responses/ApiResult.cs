using System;
using System.Collections.Generic;

namespace AnSinhSo.Shared.Responses
{
    /// <summary>
    /// Lớp biểu diễn phản hồi API chuẩn không có dữ liệu trả về.
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// Trạng thái thành công của API.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Thông điệp phản hồi từ hệ thống.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Danh sách các lỗi phát sinh (nếu có).
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Mã theo vết request, liên kết với CorrelationId.
        /// </summary>
        public string TraceId { get; set; } = string.Empty;

        /// <summary>
        /// Thời điểm phản hồi.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Tạo phản hồi thành công.
        /// </summary>
        public static ApiResult SuccessResult(string message = "Success", string traceId = "")
        {
            return new ApiResult
            {
                Success = true,
                Message = message,
                TraceId = traceId
            };
        }

        /// <summary>
        /// Tạo phản hồi thất bại.
        /// </summary>
        public static ApiResult FailureResult(List<string> errors, string message = "Failure", string traceId = "")
        {
            return new ApiResult
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                TraceId = traceId
            };
        }

        /// <summary>
        /// Tạo phản hồi thất bại với một lỗi duy nhất.
        /// </summary>
        public static ApiResult FailureResult(string error, string message = "Failure", string traceId = "")
        {
            return new ApiResult
            {
                Success = false,
                Message = message,
                Errors = new List<string> { error },
                TraceId = traceId
            };
        }
    }
}
