using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AnSinhSo.API.Middlewares
{
    /// <summary>
    /// Middleware thêm các HTTP Security Headers chuẩn vào mọi response.
    /// </summary>
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Khởi tạo SecurityHeadersMiddleware.
        /// </summary>
        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Thực thi xử lý Middleware.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                var headers = context.Response.Headers;

                if (!headers.ContainsKey("X-Frame-Options"))
                {
                    headers.Append("X-Frame-Options", "DENY");
                }

                if (!headers.ContainsKey("X-Content-Type-Options"))
                {
                    headers.Append("X-Content-Type-Options", "nosniff");
                }

                if (!headers.ContainsKey("Referrer-Policy"))
                {
                    headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
                }

                if (!headers.ContainsKey("Permissions-Policy"))
                {
                    headers.Append("Permissions-Policy", "geolocation=(), microphone=(), camera=()");
                }

                if (!headers.ContainsKey("Content-Security-Policy"))
                {
                    if (context.Request.Path.StartsWithSegments("/swagger"))
                    {
                        headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data:;");
                    }
                    else
                    {
                        headers.Append("Content-Security-Policy", "default-src 'none'; frame-ancestors 'none'; form-action 'none';");
                    }
                }

                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
