using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Domain.Errors;

public static class OtpErrors
{
    public static readonly Error NotFound = Error.NotFound(
        "Otp.NotFound", 
        "Không tìm thấy phiên xác thực OTP khả dụng.");

    public static readonly Error Expired = Error.Validation(
        "Otp.Expired", 
        "Mã OTP đã hết hạn.");

    public static readonly Error Invalid = Error.Validation(
        "Otp.Invalid", 
        "Mã OTP không hợp lệ.");

    public static readonly Error ExceedMaxAttempts = Error.Validation(
        "Otp.ExceedMaxAttempts", 
        "Bạn đã nhập sai quá số lần cho phép.");

    public static readonly Error RateLimited = Error.Validation(
        "Otp.RateLimited", 
        "Yêu cầu gửi OTP quá nhanh, vui lòng thử lại sau.");
        
    public static readonly Error AlreadyUsed = Error.Validation(
        "Otp.AlreadyUsed", 
        "Mã OTP đã được sử dụng.");
}
