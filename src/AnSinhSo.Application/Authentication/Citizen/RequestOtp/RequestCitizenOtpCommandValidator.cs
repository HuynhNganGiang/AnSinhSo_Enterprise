using FluentValidation;

namespace AnSinhSo.Application.Authentication.Citizen.RequestOtp;

public sealed class RequestCitizenOtpCommandValidator : AbstractValidator<RequestCitizenOtpCommand>
{
    public RequestCitizenOtpCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Số điện thoại không được để trống.")
            .Matches(@"^(84[3|5|7|8|9]|0[3|5|7|8|9])([0-9]{8})$").WithMessage("Số điện thoại không hợp lệ. Ví dụ: 0987654321 hoặc 84987654321.");
    }
}
