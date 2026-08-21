using System.Linq;
using AnSinhSo.Domain.SeedWork.Exceptions;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate.BusinessRules;

/// <summary>
/// Business Rule: S? CCCD ph?i h?p l?.
/// </summary>
public sealed class CitizenNumberMustBeValidRule : IBusinessRule
{
    private readonly string _citizenNumber;

    /// <summary>
    /// Kh?i t?o rule.
    /// </summary>
    public CitizenNumberMustBeValidRule(string citizenNumber)
    {
        _citizenNumber = citizenNumber;
    }

    /// <inheritdoc />
    public string Message => "S? CCCD không h?p l?.";

    /// <inheritdoc />
    public bool IsBroken() => string.IsNullOrWhiteSpace(_citizenNumber) || _citizenNumber.Length != 12 || !_citizenNumber.All(char.IsDigit);
}
