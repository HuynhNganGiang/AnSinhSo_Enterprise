using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace AnSinhSo.API.Middlewares
{
    /// <summary>
    /// Middleware quản lý Correlation ID cho các request HTTP để hỗ trợ giám sát và ghi nhật ký có hệ thống.
    /// </summary>
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdHeaderKey = "X-Correlation-ID";

        /// <summary>
        /// Khởi tạo CorrelationIdMiddleware.
        /// </summary>
        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Thực thi xử lý Middleware.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(CorrelationIdHeaderKey, out StringValues correlationIdValues))
            {
                correlationIdValues = Guid.NewGuid().ToString();
            }

            string correlationId = correlationIdValues.ToString();

            // Lưu vào HttpContext.Items để có thể dùng lại dễ dàng ở các vị trí khác
            context.Items["CorrelationId"] = correlationId;

            // Thêm Correlation ID vào response headers
            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(CorrelationIdHeaderKey))
                {
                    context.Response.Headers.Append(CorrelationIdHeaderKey, correlationId);
                }
                return Task.CompletedTask;
            });

            // Push CorrelationId vào Serilog LogContext để tự động đính kèm vào tất cả dòng log phát sinh trong request
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }
    }
}
