using FluentValidation;

namespace AnSinhSo.Application.Citizens.Commands.UpdateCitizen;

public sealed class UpdateCitizenCommandValidator : AbstractValidator<UpdateCitizenCommand>
{
    public UpdateCitizenCommandValidator()
    {
        RuleFor(x => x.CitizenId).NotEmpty();

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Họ và tên không được để trống.")
            .MaximumLength(100).WithMessage("Họ và tên không vượt quá 100 ký tự.");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Ngày sinh không được để trống.");

        RuleFor(x => x.Gender)
            .NotNull();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Số điện thoại không được để trống.")
            .Matches(@"^(0[3|5|7|8|9])+([0-9]{8})$").WithMessage("Số điện thoại không hợp lệ.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Địa chỉ không được để trống.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống.")
            .EmailAddress().WithMessage("Email không hợp lệ.");
    }
}
