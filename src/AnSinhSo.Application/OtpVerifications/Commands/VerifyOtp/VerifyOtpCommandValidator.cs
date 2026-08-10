using FluentValidation;

namespace AnSinhSo.Application.OtpVerifications.Commands.VerifyOtp;

public sealed class VerifyOtpCommandValidator : AbstractValidator<VerifyOtpCommand>
{
    public VerifyOtpCommandValidator()
    {
        RuleFor(x => x.CitizenIdentityId)
            .NotEmpty().WithMessage("CitizenIdentityId không được trống.");

        RuleFor(x => x.OtpCode)
            .NotEmpty().WithMessage("Mã OTP không được trống.")
            .Length(6).WithMessage("Mã OTP phải có 6 ký tự.")
            .Matches("^[0-9]+$").WithMessage("Mã OTP chỉ chứa ký tự số.");
    }
}
