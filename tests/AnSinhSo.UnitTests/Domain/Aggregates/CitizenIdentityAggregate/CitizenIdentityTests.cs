using System;
using System.Linq;
using Xunit;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Events;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;

namespace AnSinhSo.UnitTests.Domain.Aggregates.CitizenIdentityAggregate;

public class CitizenIdentityTests
{
    private CitizenIdentityId GenerateIdentityId() => CitizenIdentityId.Create(Guid.NewGuid());
    private CitizenId GenerateCitizenId() => new CitizenId(Guid.NewGuid());

    [Fact]
    public void Create_ShouldInitializeWithPendingVerificationStatus()
    {
        // Arrange
        var id = GenerateIdentityId();
        var citizenId = GenerateCitizenId();
        var securityStamp = "stamp123";

        // Act
        var identity = CitizenIdentity.Create(id, citizenId, securityStamp);

        // Assert
        Assert.Equal(id, identity.Id);
        Assert.Equal(citizenId, identity.CitizenId);
        Assert.Equal(IdentityStatus.Pending, identity.Status);
        Assert.Equal(securityStamp, identity.SecurityStamp);
        Assert.Equal(0, identity.FailedAttemptCount);
    }

    [Fact]
    public void VerifyPhoneNumber_WhenPending_ShouldActivateAndRaiseEvent()
    {
        // Arrange
        var identity = CitizenIdentity.Create(GenerateIdentityId(), GenerateCitizenId(), "stamp");
        var phone = PhoneNumber.Create("0912345678");
        var verifiedAt = DateTime.UtcNow;

        // Act
        identity.VerifyPhoneNumber(phone, verifiedAt);

        // Assert
        Assert.Equal(IdentityStatus.Verified, identity.Status);
        Assert.Equal(phone, identity.PrimaryPhone);
        
        var domainEvent = identity.GetDomainEvents().OfType<CitizenIdentityActivatedDomainEvent>().SingleOrDefault();
        Assert.NotNull(domainEvent);
        Assert.Equal(identity.Id, domainEvent.CitizenIdentityId);
        Assert.Equal(identity.CitizenId, domainEvent.CitizenId);
        Assert.Equal(verifiedAt, domainEvent.ActivatedAt);
    }

    [Fact]
    public void VerifyPhoneNumber_WhenNotPending_ShouldThrowException()
    {
        // Arrange
        var identity = CitizenIdentity.Create(GenerateIdentityId(), GenerateCitizenId(), "stamp");
        var phone = PhoneNumber.Create("0912345678");
        
        // First verify to make it Active
        identity.VerifyPhoneNumber(phone, DateTime.UtcNow);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => 
            identity.VerifyPhoneNumber(PhoneNumber.Create("0987654321"), DateTime.UtcNow));
        
        Assert.Contains("Cannot verify phone number when status is", ex.Message);
    }

    [Fact]
    public void LinkExternalProvider_WhenPending_ShouldActivateAndRaiseEvents()
    {
        // Arrange
        var identity = CitizenIdentity.Create(GenerateIdentityId(), GenerateCitizenId(), "stamp");
        var providerId = Guid.NewGuid();
        var linkedAt = DateTime.UtcNow;

        // Act
        identity.LinkExternalProvider(providerId, ProviderType.VNeID, "123456789012", linkedAt);

        // Assert
        Assert.Equal(IdentityStatus.Verified, identity.Status);
        Assert.Single(identity.LinkedProviders);
        
        var linkedProvider = identity.LinkedProviders.First();
        Assert.Equal(providerId, linkedProvider.Id);
        Assert.Equal(ProviderType.VNeID, linkedProvider.ProviderType);
        Assert.Equal("123456789012", linkedProvider.SubjectId);
        Assert.Equal(linkedAt, linkedProvider.LinkedAt);

        var activatedEvent = identity.GetDomainEvents().OfType<CitizenIdentityActivatedDomainEvent>().SingleOrDefault();
        Assert.NotNull(activatedEvent);

        var linkedEvent = identity.GetDomainEvents().OfType<ExternalProviderLinkedDomainEvent>().SingleOrDefault();
        Assert.NotNull(linkedEvent);
        Assert.Equal(ProviderType.VNeID, linkedEvent.ProviderType);
    }

    [Fact]
    public void LinkExternalProvider_WhenLocked_ShouldThrowException()
    {
        // Arrange
        var identity = CitizenIdentity.Create(GenerateIdentityId(), GenerateCitizenId(), "stamp");
        
        // Make it locked
        identity.RecordFailedAttempt(1, "new_stamp", DateTime.UtcNow);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => 
            identity.LinkExternalProvider(Guid.NewGuid(), ProviderType.VNeID, "sub", DateTime.UtcNow));
        
        Assert.Contains("Cannot link provider when identity is", ex.Message);
    }

    [Fact]
    public void LinkExternalProvider_SameProviderTypeTwice_ShouldThrowException()
    {
        // Arrange
        var identity = CitizenIdentity.Create(GenerateIdentityId(), GenerateCitizenId(), "stamp");
        identity.LinkExternalProvider(Guid.NewGuid(), ProviderType.VNeID, "sub1", DateTime.UtcNow);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => 
            identity.LinkExternalProvider(Guid.NewGuid(), ProviderType.VNeID, "sub2", DateTime.UtcNow));
        
        Assert.Contains("is already linked", ex.Message);
    }

    [Fact]
    public void AddTrustedDevice_UnderMaxLimit_ShouldAddDevice()
    {
        // Arrange
        var identity = CitizenIdentity.Create(GenerateIdentityId(), GenerateCitizenId(), "stamp");
        var deviceId = "device-123";
        var deviceEntityId = Guid.NewGuid();

        // Act
        identity.AddTrustedDevice(deviceEntityId, deviceId, "My Phone", DateTime.UtcNow, 5);

        // Assert
        Assert.Single(identity.TrustedDevices);
        Assert.Equal(deviceId, identity.TrustedDevices.First().DeviceId);
    }

    [Fact]
    public void AddTrustedDevice_OverMaxLimit_ShouldThrowException()
    {
        // Arrange
        var identity = CitizenIdentity.Create(GenerateIdentityId(), GenerateCitizenId(), "stamp");
        int maxDevices = 2;

        identity.AddTrustedDevice(Guid.NewGuid(), "dev1", "Phone 1", DateTime.UtcNow, maxDevices);
        identity.AddTrustedDevice(Guid.NewGuid(), "dev2", "Phone 2", DateTime.UtcNow, maxDevices);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => 
            identity.AddTrustedDevice(Guid.NewGuid(), "dev3", "Phone 3", DateTime.UtcNow, maxDevices));
        
        Assert.Contains("Maximum trusted devices", ex.Message);
    }

    [Fact]
    public void AddTrustedDevice_DuplicateDeviceId_ShouldThrowException()
    {
        // Arrange
        var identity = CitizenIdentity.Create(GenerateIdentityId(), GenerateCitizenId(), "stamp");
        identity.AddTrustedDevice(Guid.NewGuid(), "dev1", "Phone 1", DateTime.UtcNow, 5);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => 
            identity.AddTrustedDevice(Guid.NewGuid(), "dev1", "Phone 2", DateTime.UtcNow, 5));
        
        Assert.Contains("already trusted", ex.Message);
    }

    [Fact]
    public void RecordFailedAttempt_ShouldIncrementCount_AndLockIfMaxReached()
    {
        // Arrange
        var identity = CitizenIdentity.Create(GenerateIdentityId(), GenerateCitizenId(), "stamp");
        var lockedAt = DateTime.UtcNow;
        var newStamp = "new_stamp_after_lock";

        // Act
        identity.RecordFailedAttempt(3, "temp_stamp1", DateTime.UtcNow);
        identity.RecordFailedAttempt(3, "temp_stamp2", DateTime.UtcNow);
        
        Assert.Equal(2, identity.FailedAttemptCount);
        Assert.Equal(IdentityStatus.Pending, identity.Status);

        identity.RecordFailedAttempt(3, newStamp, lockedAt);

        // Assert
        Assert.Equal(3, identity.FailedAttemptCount);
        Assert.Equal(IdentityStatus.Suspended, identity.Status);
        Assert.Equal(newStamp, identity.SecurityStamp);

        var lockedEvent = identity.GetDomainEvents().OfType<CitizenIdentityLockedDomainEvent>().SingleOrDefault();
        Assert.NotNull(lockedEvent);
        Assert.Equal(lockedAt, lockedEvent.LockedAt);
        Assert.Equal(identity.Id, lockedEvent.CitizenIdentityId);
    }
}
