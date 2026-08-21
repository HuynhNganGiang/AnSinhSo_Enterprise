using System;
using AnSinhSo.Application.Authentication.Citizen.VerifyOtp;
using FluentValidation.TestHelper;
using Xunit;

namespace AnSinhSo.UnitTests.Application.Authentication.Citizen.VerifyOtp;

public class VerifyCitizenOtpCommandValidatorTests
{
    private readonly VerifyCitizenOtpCommandValidator _validator;

    public VerifyCitizenOtpCommandValidatorTests()
    {
        _validator = new VerifyCitizenOtpCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_RequestId_Is_Empty()
    {
        var command = new VerifyCitizenOtpCommand(Guid.Empty, "123456", "Dev", "Chrome", "Win", "PC", "1.1.1.1", "fp", "UTC", false);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.RequestId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Have_Error_When_OtpCode_Is_Empty(string otp)
    {
        var command = new VerifyCitizenOtpCommand(Guid.NewGuid(), otp, "Dev", "Chrome", "Win", "PC", "1.1.1.1", "fp", "UTC", false);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.OtpCode).WithErrorMessage("Mã OTP không được để trống.");
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("1234567")]
    public void Should_Have_Error_When_OtpCode_Length_Is_Invalid(string otp)
    {
        var command = new VerifyCitizenOtpCommand(Guid.NewGuid(), otp, "Dev", "Chrome", "Win", "PC", "1.1.1.1", "fp", "UTC", false);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.OtpCode).WithErrorMessage("Mã OTP phải có đúng 6 ký tự số.");
    }

    [Theory]
    [InlineData("12345a")]
    [InlineData("abcdef")]
    public void Should_Have_Error_When_OtpCode_Format_Is_Invalid(string otp)
    {
        var command = new VerifyCitizenOtpCommand(Guid.NewGuid(), otp, "Dev", "Chrome", "Win", "PC", "1.1.1.1", "fp", "UTC", false);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.OtpCode).WithErrorMessage("Mã OTP chỉ bao gồm các chữ số.");
    }

    [Fact]
    public void Should_Have_Error_When_Required_Fields_Are_Empty()
    {
        var command = new VerifyCitizenOtpCommand(Guid.NewGuid(), "123456", "", "", "", "", "", "", "", false);
        var result = _validator.TestValidate(command);
        
        result.ShouldHaveValidationErrorFor(x => x.DeviceName);
        result.ShouldHaveValidationErrorFor(x => x.Browser);
        result.ShouldHaveValidationErrorFor(x => x.OS);
        result.ShouldHaveValidationErrorFor(x => x.Platform);
        result.ShouldHaveValidationErrorFor(x => x.IpAddress);
        result.ShouldHaveValidationErrorFor(x => x.Fingerprint);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new VerifyCitizenOtpCommand(Guid.NewGuid(), "123456", "Dev", "Chrome", "Win", "PC", "1.1.1.1", "fp", "UTC", false);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
