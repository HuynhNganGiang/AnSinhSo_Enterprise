using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AnSinhSo.Shared.Responses;
using Microsoft.AspNetCore.Http;

namespace AnSinhSo.API.Middlewares
{
    /// <summary>
    /// Middleware tự động bọc kết quả phản hồi JSON của các API thành cấu trúc ApiResult chuẩn.
    /// </summary>
    public class ResponseWrapperMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Khởi tạo ResponseWrapperMiddleware.
        /// </summary>
        public ResponseWrapperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Thực thi xử lý Middleware.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;
            if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/health", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var originalBodyStream = context.Response.Body;
            using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            await _next(context);

            context.Response.Body = originalBodyStream;

            var contentType = context.Response.ContentType ?? string.Empty;
            bool isJson = contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase);
            bool isSuccess = context.Response.StatusCode >= 200 && context.Response.StatusCode < 300;

            // Không bọc nếu không phải JSON hoặc là một dạng Stream/File/Image/Zip/CSV/PDF
            if (!isSuccess || !isJson)
            {
                memStream.Position = 0;
                await memStream.CopyToAsync(originalBodyStream);
                return;
            }

            memStream.Position = 0;
            string responseBody = await new StreamReader(memStream).ReadToEndAsync();

            string traceId = context.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();

            bool isAlreadyWrapped = false;
            object? data = null;

            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Object &&
                    root.TryGetProperty("success", out _) &&
                    root.TryGetProperty("message", out _) &&
                    root.TryGetProperty("traceId", out _))
                {
                    isAlreadyWrapped = true;
                }
                else
                {
                    data = JsonSerializer.Deserialize<object>(responseBody);
                }
            }
            catch
            {
                data = responseBody;
            }

            if (!isAlreadyWrapped)
            {
                var wrappedResult = ApiResult<object>.SuccessResult(data!, "Yêu cầu xử lý thành công.", traceId);

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                string jsonResponse = JsonSerializer.Serialize(wrappedResult, options);
                context.Response.ContentType = "application/json";
                context.Response.ContentLength = System.Text.Encoding.UTF8.GetByteCount(jsonResponse);
                await context.Response.WriteAsync(jsonResponse);
            }
            else
            {
                memStream.Position = 0;
                await memStream.CopyToAsync(originalBodyStream);
            }
        }
    }
}
