using System;
using AnSinhSo.Application.Authentication.Citizen.RequestOtp;
using FluentValidation.TestHelper;
using Xunit;

namespace AnSinhSo.UnitTests.Application.Authentication.Citizen.RequestOtp;

public class RequestCitizenOtpCommandValidatorTests
{
    private readonly RequestCitizenOtpCommandValidator _validator;

    public RequestCitizenOtpCommandValidatorTests()
    {
        _validator = new RequestCitizenOtpCommandValidator();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Have_Error_When_PhoneNumber_Is_Empty(string phoneNumber)
    {
        var command = new RequestCitizenOtpCommand(phoneNumber);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
            .WithErrorMessage("Số điện thoại không được để trống.");
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("0281234567")] // Not mobile prefix
    [InlineData("098765432a")] // Invalid chars
    [InlineData("0123456789")] // Invalid prefix
    public void Should_Have_Error_When_PhoneNumber_Is_Invalid(string phoneNumber)
    {
        var command = new RequestCitizenOtpCommand(phoneNumber);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
            .WithErrorMessage("Số điện thoại không hợp lệ. Ví dụ: 0987654321 hoặc 84987654321.");
    }

    [Theory]
    [InlineData("0987654321")]
    [InlineData("84987654321")]
    [InlineData("0351234567")]
    public void Should_Not_Have_Error_When_PhoneNumber_Is_Valid(string phoneNumber)
    {
        var command = new RequestCitizenOtpCommand(phoneNumber);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
    }
}
