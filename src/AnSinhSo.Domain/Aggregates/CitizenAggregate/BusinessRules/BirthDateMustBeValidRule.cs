using System;
using AnSinhSo.Domain.SeedWork.Exceptions;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate.BusinessRules;

/// <summary>
/// Business Rule: Ngày sinh không được lớn hơn ngày hiện tại.
/// </summary>
public sealed class BirthDateMustBeValidRule : IBusinessRule
{
    private readonly DateTime _birthDate;

    /// <summary>
    /// Khởi tạo rule.
    /// </summary>
    public BirthDateMustBeValidRule(DateTime birthDate)
    {
        _birthDate = birthDate;
    }

    /// <inheritdoc />
    public string Message => "Ngày sinh không được lớn hơn ngày hiện tại.";

    /// <inheritdoc />
    public bool IsBroken() => _birthDate.Date > DateTime.UtcNow.Date;
}
