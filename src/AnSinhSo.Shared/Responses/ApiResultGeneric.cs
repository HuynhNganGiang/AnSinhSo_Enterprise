using System;
using System.Collections.Generic;

namespace AnSinhSo.Shared.Responses
{
    /// <summary>
    /// Lớp biểu diễn phản hồi API chuẩn có chứa dữ liệu trả về.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của payload trả về.</typeparam>
    public class ApiResult<T> : ApiResult
    {
        /// <summary>
        /// Dữ liệu payload của API.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Tạo phản hồi thành công có chứa dữ liệu.
        /// </summary>
        public static ApiResult<T> SuccessResult(T data, string message = "Success", string traceId = "")
        {
            return new ApiResult<T>
            {
                Success = true,
                Message = message,
                Data = data,
                TraceId = traceId
            };
        }

        /// <summary>
        /// Tạo phản hồi thất bại cho API generic.
        /// </summary>
        public static new ApiResult<T> FailureResult(List<string> errors, string message = "Failure", string traceId = "")
        {
            return new ApiResult<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                TraceId = traceId
            };
        }

        /// <summary>
        /// Tạo phản hồi thất bại cho API generic với một lỗi duy nhất.
        /// </summary>
        public static new ApiResult<T> FailureResult(string error, string message = "Failure", string traceId = "")
        {
            return new ApiResult<T>
            {
                Success = false,
                Message = message,
                Errors = new List<string> { error },
                TraceId = traceId
            };
        }
    }
}
