using System;
using System.Linq;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Authentication;

public static class PasswordPolicyValidator
{
    public static Result Validate(string password, AuthenticationOptions options)
    {
        if (string.IsNullOrEmpty(password))
            return Result.Failure(Error.Validation("Password.Empty", "Mật khẩu không được để trống."));

        if (password.Length < options.PasswordMinLength)
            return Result.Failure(Error.Validation("Password.TooShort", $"Mật khẩu phải có ít nhất {options.PasswordMinLength} ký tự."));

        if (options.PasswordRequireUppercase && !password.Any(char.IsUpper))
            return Result.Failure(Error.Validation("Password.NoUppercase", "Mật khẩu phải chứa ít nhất một chữ cái in hoa."));

        if (options.PasswordRequireLowercase && !password.Any(char.IsLower))
            return Result.Failure(Error.Validation("Password.NoLowercase", "Mật khẩu phải chứa ít nhất một chữ cái in thường."));

        if (options.PasswordRequireDigit && !password.Any(char.IsDigit))
            return Result.Failure(Error.Validation("Password.NoDigit", "Mật khẩu phải chứa ít nhất một chữ số."));

        if (options.PasswordRequireSpecialChar && !password.Any(c => !char.IsLetterOrDigit(c)))
            return Result.Failure(Error.Validation("Password.NoSpecialChar", "Mật khẩu phải chứa ít nhất một ký tự đặc biệt."));

        return Result.Success();
    }
}
