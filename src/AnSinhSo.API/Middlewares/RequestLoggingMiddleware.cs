using System.Diagnostics;
using System.Security.Claims;
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
            var sw = Stopwatch.StartNew();
            var request = context.Request;

            await _next(context);

            sw.Stop();

            var correlationId = context.Items["CorrelationId"]?.ToString() ?? string.Empty;
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = request.Headers.UserAgent.ToString();
            var userId = context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";
            var requestId = context.TraceIdentifier;

            _logger.LogInformation(
                "HTTP Request Completed: {Method} {Path} | StatusCode: {StatusCode} | ElapsedTime: {ElapsedMs}ms | CorrelationId: {CorrelationId} | IP: {IP} | UserAgent: {UserAgent} | UserId: {UserId} | RequestId: {RequestId}",
                request.Method,
                request.Path,
                context.Response.StatusCode,
                sw.ElapsedMilliseconds,
                correlationId,
                ip,
                userAgent,
                userId,
                requestId);
        }
    }
}
