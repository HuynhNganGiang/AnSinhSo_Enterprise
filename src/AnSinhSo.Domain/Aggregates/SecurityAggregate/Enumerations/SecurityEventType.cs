namespace AnSinhSo.Domain.Aggregates.SecurityAggregate.Enumerations;

public enum SecurityEventType
{
    LOGIN_SUCCESS,
    LOGIN_FAILED,
    OTP_SENT,
    OTP_FAILED,
    PASSWORD_CHANGED,
    USER_LOCKED,
    USER_UNLOCKED,
    ROLE_CHANGED,
    ZALO_LINKED
}
