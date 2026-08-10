using System;
using System.Text.RegularExpressions;

namespace AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;

public sealed record PhoneNumber
{
    public string Value { get; }

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Phone number cannot be empty.", nameof(value));
        }

        // Basic validation for Vietnam phone numbers
        var regex = new Regex(@"^(0|\+84)(3|5|7|8|9)[0-9]{8}$");
        if (!regex.IsMatch(value))
        {
            throw new ArgumentException("Invalid phone number format.", nameof(value));
        }

        Value = value;
    }

    public static PhoneNumber Create(string value) => new(value);

    public override string ToString() => Value;
}
