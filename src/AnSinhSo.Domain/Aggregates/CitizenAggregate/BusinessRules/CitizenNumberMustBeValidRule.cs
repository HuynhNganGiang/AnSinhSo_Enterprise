using System.Linq;
using AnSinhSo.Domain.SeedWork.Exceptions;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate.BusinessRules;

/// <summary>
/// Business Rule: Số CCCD phải hợp lệ.
/// </summary>
public sealed class CitizenNumberMustBeValidRule : IBusinessRule
{
    private readonly string _citizenNumber;

    /// <summary>
    /// Khởi tạo rule.
    /// </summary>
    public CitizenNumberMustBeValidRule(string citizenNumber)
    {
        _citizenNumber = citizenNumber;
    }

    /// <inheritdoc />
    public string Message => "Số CCCD không hợp lệ.";

    /// <inheritdoc />
    public bool IsBroken() => string.IsNullOrWhiteSpace(_citizenNumber) || _citizenNumber.Length != 12 || !_citizenNumber.All(char.IsDigit);
}
