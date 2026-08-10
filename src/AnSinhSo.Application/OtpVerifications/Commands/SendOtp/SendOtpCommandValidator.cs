using FluentValidation;

namespace AnSinhSo.Application.OtpVerifications.Commands.SendOtp;

public sealed class SendOtpCommandValidator : AbstractValidator<SendOtpCommand>
{
    public SendOtpCommandValidator()
    {
        RuleFor(x => x.CitizenIdentityId)
            .NotEmpty().WithMessage("CitizenIdentityId không được trống.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Số điện thoại không được trống.")
            .Matches(@"^(0[3|5|7|8|9])+([0-9]{8})$").WithMessage("Số điện thoại không đúng định dạng.");

        RuleFor(x => x.Purpose)
            .NotEmpty().WithMessage("Mục đích OTP không được trống.");
    }
}
