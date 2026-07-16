using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AnSinhSo.API.Middlewares
{
    /// <summary>
    /// Middleware ghi nhật ký chi tiết cho mọi HTTP request đầu vào và kết quả response.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        /// <summary>
        /// Khởi tạo RequestLoggingMiddleware.
        /// </summary>
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Thực thi xử lý Middleware.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            
            _logger.LogInformation("HTTP Request: {Method} {Path}{QueryString} | ClientIP: {IP} | UserAgent: {UserAgent}",
                request.Method,
                request.Path,
                request.QueryString.HasValue ? request.QueryString.Value : string.Empty,
                context.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                request.Headers.UserAgent.ToString());

            await _next(context);

            _logger.LogInformation("HTTP Response: {Method} {Path} status resolved to {StatusCode}",
                request.Method,
                request.Path,
                context.Response.StatusCode);
        }
    }
}
