using FluentValidation;

namespace AnSinhSo.Application.Authentication.Citizen.VerifyOtp;

public sealed class VerifyCitizenOtpCommandValidator : AbstractValidator<VerifyCitizenOtpCommand>
{
    public VerifyCitizenOtpCommandValidator()
    {
        RuleFor(x => x.RequestId)
            .NotEmpty().WithMessage("RequestId không được để trống.");

        RuleFor(x => x.OtpCode)
            .NotEmpty().WithMessage("Mã OTP không được để trống.")
            .Length(6).WithMessage("Mã OTP phải có đúng 6 ký tự số.")
            .Matches("^[0-9]*$").WithMessage("Mã OTP chỉ bao gồm các chữ số.");
            
        RuleFor(x => x.DeviceName)
            .NotEmpty().WithMessage("DeviceName không được để trống.");
            
        RuleFor(x => x.Browser)
            .NotEmpty().WithMessage("Browser không được để trống.");
            
        RuleFor(x => x.OS)
            .NotEmpty().WithMessage("OS không được để trống.");
            
        RuleFor(x => x.Platform)
            .NotEmpty().WithMessage("Platform không được để trống.");
            
        RuleFor(x => x.IpAddress)
            .NotEmpty().WithMessage("IpAddress không được để trống.");
            
        RuleFor(x => x.Fingerprint)
            .NotEmpty().WithMessage("Fingerprint không được để trống.");
    }
}
