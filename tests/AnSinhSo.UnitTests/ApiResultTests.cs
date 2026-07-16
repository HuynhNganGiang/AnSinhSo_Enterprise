using AnSinhSo.Shared.Responses;
using Xunit;

namespace AnSinhSo.UnitTests
{
    public class ApiResultTests
    {
        [Fact]
        public void SuccessResult_ShouldCreateSuccessfulApiResult()
        {
            // Arrange
            string message = "Yêu cầu xử lý thành công.";
            string traceId = "test-trace-id";

            // Act
            var result = ApiResult.SuccessResult(message, traceId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(message, result.Message);
            Assert.Equal(traceId, result.TraceId);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void FailureResult_ShouldCreateFailureApiResult()
        {
            // Arrange
            string error = "Lỗi xác thực dữ liệu.";
            string message = "Yêu cầu xử lý thất bại.";
            string traceId = "test-trace-id";

            // Act
            var result = ApiResult.FailureResult(error, message, traceId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(message, result.Message);
            Assert.Equal(traceId, result.TraceId);
            Assert.Single(result.Errors);
            Assert.Equal(error, result.Errors[0]);
        }
    }
}