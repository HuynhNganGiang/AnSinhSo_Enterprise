using System;
using System.Linq;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate.Events;
using Xunit;

namespace AnSinhSo.UnitTests.Domain.OtpVerificationAggregate;

public class OtpVerificationTests
{
    private readonly CitizenIdentityId _citizenIdentityId = CitizenIdentityId.Create(Guid.NewGuid());
    private readonly PhoneNumber _phoneNumber = PhoneNumber.Create("0901234567");
    private readonly string _validCodeHash = "hashed_123456";
    private readonly DateTime _createdAt = DateTime.UtcNow;

    private OtpVerification CreateValidOtp()
    {
        return OtpVerification.Create(
            _citizenIdentityId,
            Guid.NewGuid(),
            _validCodeHash,
            _phoneNumber,
            _createdAt.AddMinutes(5)
        );
    }

    [Fact]
    public void Create_Should_Initialize_With_Pending_Status()
    {
        // Act
        var otp = CreateValidOtp();

        // Assert
        Assert.Equal(_citizenIdentityId, otp.CitizenIdentityId);
        Assert.Equal(_phoneNumber, otp.TargetPhone);
        Assert.Equal(_validCodeHash, otp.CodeHash);
        Assert.Equal(OtpStatus.Pending, otp.Status);
        Assert.Equal(0, otp.FailedAttemptCount);
        Assert.Empty(otp.GetDomainEvents());
    }

    [Fact]
    public void Verify_With_Valid_Code_Before_Expiration_Should_Succeed_And_Set_Status_To_Used()
    {
        // Arrange
        var otp = CreateValidOtp();
        var now = _createdAt.AddMinutes(1);

        // Act
        otp.Verify(_validCodeHash, maxAttempts: 3, now);

        // Assert
        Assert.Equal(OtpStatus.Verified, otp.Status);
        Assert.Equal(0, otp.FailedAttemptCount);
        
        var domainEvent = otp.GetDomainEvents().OfType<OtpVerifiedDomainEvent>().SingleOrDefault();
        Assert.NotNull(domainEvent);
        Assert.Equal(otp.Id, domainEvent!.OtpVerificationId);
        Assert.Equal(_citizenIdentityId, domainEvent.CitizenIdentityId);
    }

    [Fact]
    public void Verify_With_Expired_Time_Should_Throw_And_Set_Status_To_Expired()
    {
        // Arrange
        var otp = CreateValidOtp();
        var now = _createdAt.AddMinutes(10); // After 5 minutes expiration

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => otp.Verify(_validCodeHash, 3, now));

        // Assert
        Assert.Contains("expired", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(OtpStatus.Expired, otp.Status);
    }

    [Fact]
    public void Verify_With_Invalid_Code_Should_Increment_FailedCount_And_Throw()
    {
        // Arrange
        var otp = CreateValidOtp();
        var now = _createdAt.AddMinutes(1);

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => otp.Verify("wrong_hash", maxAttempts: 3, now));

        // Assert
        Assert.Contains("Invalid", ex.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(1, otp.FailedAttemptCount);
        Assert.Equal(OtpStatus.Pending, otp.Status);
    }

    [Fact]
    public void Verify_With_Invalid_Code_Exceeding_MaxAttempts_Should_Revoke_OTP()
    {
        // Arrange
        var otp = CreateValidOtp();
        var now = _createdAt.AddMinutes(1);
        int maxAttempts = 3;

        // Act & Assert
        // Attempt 1
        Assert.Throws<InvalidOperationException>(() => otp.Verify("wrong_hash", maxAttempts, now));
        Assert.Equal(OtpStatus.Pending, otp.Status);

        // Attempt 2
        Assert.Throws<InvalidOperationException>(() => otp.Verify("wrong_hash", maxAttempts, now));
        Assert.Equal(OtpStatus.Pending, otp.Status);

        // Attempt 3 (Should exceed)
        Assert.Throws<InvalidOperationException>(() => otp.Verify("wrong_hash", maxAttempts, now));
        Assert.Equal(OtpStatus.Locked, otp.Status);
        Assert.Equal(3, otp.FailedAttemptCount);
    }

    [Fact]
    public void Verify_When_Already_Used_Should_Throw_One_Time_Usage_Rule()
    {
        // Arrange
        var otp = CreateValidOtp();
        var now = _createdAt.AddMinutes(1);
        
        // Use it first time
        otp.Verify(_validCodeHash, maxAttempts: 3, now);

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => otp.Verify(_validCodeHash, maxAttempts: 3, now.AddSeconds(10)));

        // Assert
        Assert.Contains("This OTP has already been verified/used.", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Revoke_Should_Change_Status_To_Revoked_If_Pending()
    {
        // Arrange
        var otp = CreateValidOtp();

        // Act
        otp.Revoke("New OTP generated");

        // Assert
        Assert.Equal(OtpStatus.Cancelled, otp.Status);
    }
}
