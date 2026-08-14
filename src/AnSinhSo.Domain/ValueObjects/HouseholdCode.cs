using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.ValueObjects;

public sealed class HouseholdCode : ValueObject
{
    public string Value { get; }

    public HouseholdCode(string value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
