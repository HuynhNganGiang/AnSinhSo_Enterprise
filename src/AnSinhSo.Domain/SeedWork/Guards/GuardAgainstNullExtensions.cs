using System;
using System.Diagnostics.CodeAnalysis;
using AnSinhSo.Domain.SeedWork.Exceptions;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Domain.SeedWork.Guards;

/// <summary>
/// Các phương thức mở rộng bảo vệ chống lại giá trị Null.
/// </summary>
public static class GuardAgainstNullExtensions
{
    /// <summary>
    /// Kiểm tra giá trị đầu vào có bị Null hay không. Nếu Null sẽ ném ra DomainException.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu.</typeparam>
    /// <param name="guardClause">IGuardClause.</param>
    /// <param name="input">Giá trị cần kiểm tra.</param>
    /// <param name="parameterName">Tên tham số bị lỗi.</param>
    /// <param name="message">Thông báo lỗi tùy chỉnh (tùy chọn).</param>
    /// <returns>Chính giá trị đó nếu không Null.</returns>
    /// <exception cref="DomainException">Ngoại lệ miền ném ra khi giá trị Null.</exception>
    public static T Null<T>(this IGuardClause guardClause, [NotNull] T? input, string parameterName, string? message = null)
    {
        if (input is null)
        {
            var errorMessage = message ?? $"Tham số {parameterName} không được phép rỗng.";
            // Sử dụng một lớp kế thừa DomainException hoặc throw trực tiếp (vì DomainException là abstract).
            // Do DomainException là abstract, ở đây ta tạo một class local hoặc ném BusinessRuleValidationException 
            // hoặc đơn giản là tạo ra một ngoại lệ kế thừa tạm (nếu kiến trúc cho phép).
            // Tuy nhiên, theo yêu cầu, DomainException là abstract. Ta sẽ ném ra một lỗi thông qua một lớp kế thừa.
            throw new GuardException(errorMessage);
        }

        return input;
    }

    private class GuardException : DomainException
    {
        public GuardException(string message) : base(message) { }
    }
}
