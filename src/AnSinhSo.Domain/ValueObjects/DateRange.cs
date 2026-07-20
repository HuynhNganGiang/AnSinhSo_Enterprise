using System;
using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.ValueObjects;

/// <summary>
/// Value Object đại diện cho khoảng thời gian.
/// </summary>
public sealed class DateRange : ValueObject
{
    /// <summary>
    /// Ngày bắt đầu.
    /// </summary>
    public DateTime StartDate { get; }

    /// <summary>
    /// Ngày kết thúc.
    /// </summary>
    public DateTime EndDate { get; }

    private DateRange(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>
    /// Khởi tạo DateRange.
    /// </summary>
    public static Result<DateRange> Create(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
        {
            return Result.Failure<DateRange>(Error.Validation("DateRange.Invalid", "Ngày kết thúc không được nhỏ hơn ngày bắt đầu."));
        }

        return Result.Success(new DateRange(startDate, endDate));
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }
}
