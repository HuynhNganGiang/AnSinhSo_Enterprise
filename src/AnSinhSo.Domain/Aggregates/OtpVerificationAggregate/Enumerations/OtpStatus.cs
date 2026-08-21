namespace AnSinhSo.Domain.Aggregates.OtpVerificationAggregate.Enumerations;

public enum OtpStatus
{
    Pending = 1,
    Used = 2,
    Expired = 3,
    Revoked = 4,
    Verified = 5,
    Cancelled = 6,
    Locked = 7
}
