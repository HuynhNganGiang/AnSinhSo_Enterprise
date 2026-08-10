using System;

namespace AnSinhSo.Domain.Aggregates.OtpVerificationAggregate.ValueObjects;

public readonly record struct OtpVerificationId
{
    public Guid Value { get; }

    public OtpVerificationId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("OtpVerificationId cannot be empty.", nameof(value));
        Value = value;
    }

    public static OtpVerificationId Create(Guid value) => new(value);
    public static OtpVerificationId New() => new(Guid.NewGuid());
}
