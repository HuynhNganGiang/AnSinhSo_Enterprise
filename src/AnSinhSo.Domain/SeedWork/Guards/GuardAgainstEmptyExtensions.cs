using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AnSinhSo.Domain.SeedWork.Exceptions;

namespace AnSinhSo.Domain.SeedWork.Guards;

/// <summary>
/// Các phương thức mở rộng bảo vệ chống lại chuỗi hoặc tập hợp rỗng.
/// </summary>
public static class GuardAgainstEmptyExtensions
{
    /// <summary>
    /// Kiểm tra chuỗi có rỗng hay không. Nếu rỗng sẽ ném ra DomainException.
    /// </summary>
    public static string Empty(this IGuardClause guardClause, [NotNull] string? input, string parameterName, string? message = null)
    {
        guardClause.Null(input, parameterName, message);

        if (input == string.Empty || string.IsNullOrWhiteSpace(input))
        {
            var errorMessage = message ?? $"Tham số {parameterName} không được phép rỗng hoặc chỉ chứa khoảng trắng.";
            throw new GuardException(errorMessage);
        }

        return input;
    }

    /// <summary>
    /// Kiểm tra collection có rỗng hay không. Nếu rỗng sẽ ném ra DomainException.
    /// </summary>
    public static IEnumerable<T> Empty<T>(this IGuardClause guardClause, [NotNull] IEnumerable<T>? input, string parameterName, string? message = null)
    {
        guardClause.Null(input, parameterName, message);

        if (!input.Any())
        {
            var errorMessage = message ?? $"Danh sách {parameterName} không được phép rỗng.";
            throw new GuardException(errorMessage);
        }

        return input;
    }

    private class GuardException : DomainException
    {
        public GuardException(string message) : base(message) { }
    }
}
