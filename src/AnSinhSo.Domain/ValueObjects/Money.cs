using System.Collections.Generic;
using AnSinhSo.Domain.Enumerations;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.ValueObjects;

/// <summary>
/// Value Object đại diện cho tiền tệ.
/// </summary>
public sealed class Money : ValueObject
{
    /// <summary>
    /// Số tiền.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Loại tiền tệ (Enumeration).
    /// </summary>
    public Currency Currency { get; }

    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// Khởi tạo Money.
    /// </summary>
    public static Result<Money> Create(decimal amount, Currency currency)
    {
        if (amount < 0)
        {
            return Result.Failure<Money>(Error.Validation("Money.NegativeAmount", "Số tiền không được âm."));
        }

        if (currency is null)
        {
            return Result.Failure<Money>(Error.Validation("Money.CurrencyNull", "Loại tiền tệ không được để trống."));
        }

        return Result.Success(new Money(amount, currency));
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
