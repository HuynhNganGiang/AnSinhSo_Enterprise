using System;
using System.Collections.Generic;
using AnSinhSo.Shared.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers
{
    /// <summary>
    /// Controller cơ sở cho tất cả các API Endpoint của AnSinhSo.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Lấy TraceId (CorrelationId) của request hiện tại.
        /// </summary>
        protected string TraceId => HttpContext.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();

        /// <summary>
        /// Trả về kết quả thành công kèm dữ liệu.
        /// </summary>
        protected ActionResult<ApiResult<T>> Success<T>(T data, string message = "Yêu cầu xử lý thành công.")
        {
            return Ok(ApiResult<T>.SuccessResult(data, message, TraceId));
        }

        /// <summary>
        /// Trả về kết quả thành công không có dữ liệu.
        /// </summary>
        protected ActionResult<ApiResult> Success(string message = "Yêu cầu xử lý thành công.")
        {
            return Ok(ApiResult.SuccessResult(message, TraceId));
        }

        /// <summary>
        /// Trả về kết quả thất bại với danh sách lỗi và mã trạng thái tương ứng.
        /// </summary>
        protected ActionResult<ApiResult> Failure(List<string> errors, string message = "Yêu cầu xử lý thất bại.", int statusCode = StatusCodes.Status400BadRequest)
        {
            var result = ApiResult.FailureResult(errors, message, TraceId);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Trả về kết quả thất bại với một lỗi duy nhất và mã trạng thái tương ứng.
        /// </summary>
        protected ActionResult<ApiResult> Failure(string error, string message = "Yêu cầu xử lý thất bại.", int statusCode = StatusCodes.Status400BadRequest)
        {
            var result = ApiResult.FailureResult(error, message, TraceId);
            return StatusCode(statusCode, result);
        }
    }
}
