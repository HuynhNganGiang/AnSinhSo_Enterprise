using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AnSinhSo.API.Middlewares
{
    /// <summary>
    /// Middleware đo lường và ghi nhận thời gian xử lý của từng HTTP request.
    /// </summary>
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;
        private const string ResponseTimeHeaderKey = "X-Response-Time-Ms";

        /// <summary>
        /// Khởi tạo RequestTimingMiddleware.
        /// </summary>
        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Thực thi xử lý Middleware.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            context.Response.OnStarting(() =>
            {
                stopwatch.Stop();
                long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                
                if (!context.Response.Headers.ContainsKey(ResponseTimeHeaderKey))
                {
                    context.Response.Headers.Append(ResponseTimeHeaderKey, elapsedMilliseconds.ToString());
                }

                _logger.LogInformation("HTTP {Method} {Path} responded in {ElapsedMs} ms",
                    context.Request.Method,
                    context.Request.Path,
                    elapsedMilliseconds);

                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
