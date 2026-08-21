using System;
using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.ValueObjects;

/// <summary>
/// Value Object d?i di?n cho kho?ng th?i gian.
/// </summary>
public sealed class DateRange : ValueObject
{
    /// <summary>
    /// Ngày b?t d?u.
    /// </summary>
    public DateTime StartDate { get; }

    /// <summary>
    /// Ngày k?t thúc.
    /// </summary>
    public DateTime EndDate { get; }

    private DateRange(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>
    /// Kh?i t?o DateRange.
    /// </summary>
    public static Result<DateRange> Create(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
        {
            return Result.Failure<DateRange>(Error.Validation("DateRange.Invalid", "Ngày k?t thúc không du?c nh? hon ngày b?t d?u."));
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
