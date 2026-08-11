using FluentValidation;

namespace AnSinhSo.Application.Authentication.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Số điện thoại không được trống.")
            .Matches(@"^(0[3|5|7|8|9])+([0-9]{8})$").WithMessage("Số điện thoại không đúng định dạng.");

        RuleFor(x => x.OtpCode)
            .NotEmpty().WithMessage("Mã OTP không được trống.")
            .Length(6).WithMessage("Mã OTP phải có 6 ký tự.");

        RuleFor(x => x.IpAddress).NotEmpty().WithMessage("IP Address không được trống.");
        RuleFor(x => x.UserAgent).NotEmpty().WithMessage("User Agent không được trống.");
    }
}
