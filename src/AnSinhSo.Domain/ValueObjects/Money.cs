using System.Collections.Generic;
using AnSinhSo.Domain.Enumerations;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.ValueObjects;

/// <summary>
/// Value Object d?i di?n cho ti?n t?.
/// </summary>
public sealed class Money : ValueObject
{
    /// <summary>
    /// S? ti?n.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Lo?i ti?n t? (Enumeration).
    /// </summary>
    public Currency Currency { get; }

    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// Kh?i t?o Money.
    /// </summary>
    public static Result<Money> Create(decimal amount, Currency currency)
    {
        if (amount < 0)
        {
            return Result.Failure<Money>(Error.Validation("Money.NegativeAmount", "S? ti?n không du?c âm."));
        }

        if (currency is null)
        {
            return Result.Failure<Money>(Error.Validation("Money.CurrencyNull", "Lo?i ti?n t? không du?c d? tr?ng."));
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
