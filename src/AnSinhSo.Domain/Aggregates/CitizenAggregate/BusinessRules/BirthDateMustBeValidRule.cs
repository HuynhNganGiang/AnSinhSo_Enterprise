using System;
using AnSinhSo.Domain.SeedWork.Exceptions;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate.BusinessRules;

/// <summary>
/// Business Rule: Ngày sinh không du?c l?n hon ngày hi?n t?i.
/// </summary>
public sealed class BirthDateMustBeValidRule : IBusinessRule
{
    private readonly DateTime _birthDate;

    /// <summary>
    /// Kh?i t?o rule.
    /// </summary>
    public BirthDateMustBeValidRule(DateTime birthDate)
    {
        _birthDate = birthDate;
    }

    /// <inheritdoc />
    public string Message => "Ngày sinh không du?c l?n hon ngày hi?n t?i.";

    /// <inheritdoc />
    public bool IsBroken() => _birthDate.Date > DateTime.UtcNow.Date;
}
