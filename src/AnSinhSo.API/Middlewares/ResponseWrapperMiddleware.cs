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
            var originalBodyStream = context.Response.Body;

            // Bỏ qua việc bọc cho các Endpoint tài liệu hoặc kiểm tra sức khỏe
            var path = context.Request.Path.Value ?? string.Empty;
            if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/health", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            await _next(context);

            context.Response.Body = originalBodyStream;

            if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300 &&
                context.Response.ContentType != null && context.Response.ContentType.Contains("application/json"))
            {
                memStream.Position = 0;
                string responseBody = await new StreamReader(memStream).ReadToEndAsync();

                // Lấy Trace ID đã lưu trong CorrelationIdMiddleware
                string traceId = context.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();

                bool isAlreadyWrapped = false;
                object? data = null;

                try
                {
                    // Thử parse để kiểm tra xem đã được bọc chưa
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
                    // Nếu không parse được thành JSON object hợp lệ, coi như là chuỗi raw và bọc lại
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
            else
            {
                memStream.Position = 0;
                await memStream.CopyToAsync(originalBodyStream);
            }
        }
    }
}
