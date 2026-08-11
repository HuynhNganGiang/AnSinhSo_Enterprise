using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Domain.Errors;

public static class SessionErrors
{
    public static readonly Error NotFound = Error.NotFound(
        "Session.NotFound",
        "Phiên đăng nhập không tồn tại.");

    public static readonly Error Expired = Error.Validation(
        "Session.Expired",
        "Phiên đăng nhập đã hết hạn.");

    public static readonly Error Revoked = Error.Validation(
        "Session.Revoked",
        "Phiên đăng nhập đã bị thu hồi.");

    public static readonly Error Compromised = Error.Conflict(
        "Session.Compromised",
        "Phát hiện rủi ro bảo mật (Replay Attack). Toàn bộ phiên thuộc thiết bị đã bị vô hiệu hoá.");

    public static readonly Error LimitExceeded = Error.Conflict(
        "Session.LimitExceeded",
        "Vượt quá số lượng thiết bị đăng nhập cho phép.");
}
