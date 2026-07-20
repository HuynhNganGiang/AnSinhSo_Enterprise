using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AnSinhSo.Shared.Exceptions;
using AnSinhSo.Shared.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AnSinhSo.API.Middlewares
{
    /// <summary>
    /// Middleware xử lý ngoại lệ toàn cục của ứng dụng, đảm bảo phản hồi lỗi luôn tuân thủ chuẩn ApiResult.
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        /// <summary>
        /// Khởi tạo GlobalExceptionMiddleware.
        /// </summary>
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Thực thi xử lý Middleware.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var message = "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau.";
            var errors = new List<string>();

            // Lấy Trace ID từ Correlation ID đã được thiết lập ở CorrelationIdMiddleware
            string traceId = context.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();

            var exceptionName = exception.GetType().Name;

            switch (exceptionName)
            {
                case "ValidationException":
                    statusCode = StatusCodes.Status400BadRequest;
                    message = exception.Message;
                    break;
                case "UnauthorizedException":
                    statusCode = StatusCodes.Status401Unauthorized;
                    message = exception.Message;
                    break;
                case "ForbiddenException":
                    statusCode = StatusCodes.Status403Forbidden;
                    message = exception.Message;
                    break;
                case "NotFoundException":
                    statusCode = StatusCodes.Status404NotFound;
                    message = exception.Message;
                    break;
                case "ConflictException":
                    statusCode = StatusCodes.Status409Conflict;
                    message = exception.Message;
                    break;
                default:
                    if (exception is AppException appEx)
                    {
                        statusCode = appEx.StatusCode;
                        message = appEx.Message;
                    }
                    break;
            }

            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(exception, "Unhandled exception caught [TraceId: {TraceId}]: {Message}", traceId, exception.Message);
            }
            else
            {
                _logger.LogWarning(exception, "{ExceptionType} caught [TraceId: {TraceId}]: {Message}", exceptionName, traceId, exception.Message);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var result = ApiResult.FailureResult(errors, message, traceId);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(result, options));
        }
    }
}
