using System;
using AnSinhSo.Domain.SeedWork.Exceptions;

namespace AnSinhSo.Domain.SeedWork.Guards;

/// <summary>
/// Phương thức mở rộng bảo vệ chống lại dữ liệu không thỏa mãn điều kiện nhất định.
/// </summary>
public static class GuardAgainstInvalidExtensions
{
    /// <summary>
    /// Ném ngoại lệ miền nếu predicate trả về false.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu.</typeparam>
    /// <param name="guardClause">IGuardClause.</param>
    /// <param name="input">Giá trị cần kiểm tra.</param>
    /// <param name="predicate">Hàm kiểm tra điều kiện (trả về true là hợp lệ).</param>
    /// <param name="parameterName">Tên tham số.</param>
    /// <param name="message">Thông báo lỗi.</param>
    /// <returns>Chính giá trị đó nếu hợp lệ.</returns>
    public static T Invalid<T>(this IGuardClause guardClause, T input, Func<T, bool> predicate, string parameterName, string? message = null)
    {
        if (!predicate(input))
        {
            var errorMessage = message ?? $"Tham số {parameterName} có giá trị không hợp lệ.";
            throw new GuardException(errorMessage);
        }

        return input;
    }

    private class GuardException : DomainException
    {
        public GuardException(string message) : base(message) { }
    }
}
