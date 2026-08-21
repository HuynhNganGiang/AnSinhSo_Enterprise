using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Notifications;
using AnSinhSo.Application.Abstractions.Security;
using AnSinhSo.Application.OtpVerifications.Commands.SendOtp;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate.Enumerations;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.Interfaces;
using Moq;
using Xunit;

namespace AnSinhSo.UnitTests.Application.OtpVerifications.Commands;

public class SendOtpCommandHandlerTests
{
    private readonly Mock<ICitizenIdentityRepository> _citizenIdentityRepoMock;
    private readonly Mock<IOtpVerificationRepository> _otpVerificationRepoMock;
    private readonly Mock<IOtpGenerator> _otpGeneratorMock;
    private readonly Mock<IHashProvider> _hashProviderMock;
    private readonly Mock<IOtpNotificationService> _notificationServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly SendOtpCommandHandler _handler;

    public SendOtpCommandHandlerTests()
    {
        _citizenIdentityRepoMock = new Mock<ICitizenIdentityRepository>();
        _otpVerificationRepoMock = new Mock<IOtpVerificationRepository>();
        _otpGeneratorMock = new Mock<IOtpGenerator>();
        _hashProviderMock = new Mock<IHashProvider>();
        _notificationServiceMock = new Mock<IOtpNotificationService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new SendOtpCommandHandler(
            _citizenIdentityRepoMock.Object,
            _otpVerificationRepoMock.Object,
            _otpGeneratorMock.Object,
            _hashProviderMock.Object,
            _notificationServiceMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenIdentityNotFound()
    {
        // Arrange
        var command = new SendOtpCommand(Guid.NewGuid(), "0901234567", "Activation");
        
        _citizenIdentityRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<CitizenIdentityId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CitizenIdentity?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(IdentityErrors.IdentityNotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenIdentityNotPending()
    {
        // Arrange
        var command = new SendOtpCommand(Guid.NewGuid(), "0901234567", "Activation");
        var citizenId = new CitizenId(Guid.NewGuid());
        var identity = CitizenIdentity.Create(CitizenIdentityId.Create(command.CitizenIdentityId), citizenId, "stamp", PhoneNumber.Create(command.PhoneNumber));
        
        // Simulate changing status directly via VerifyPhoneNumber
        identity.VerifyPhoneNumber(PhoneNumber.Create(command.PhoneNumber), DateTime.UtcNow);

        _citizenIdentityRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<CitizenIdentityId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(identity);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Identity.NotPending", result.Error.Code);
    }

    [Fact]
    public async Task Handle_ShouldRevokeOldOtpAndCreateNew_WhenIdentityIsPending()
    {
        // Arrange
        var command = new SendOtpCommand(Guid.NewGuid(), "0901234567", "Activation");
        var citizenId = new CitizenId(Guid.NewGuid());
        var identityId = CitizenIdentityId.Create(command.CitizenIdentityId);
        var targetPhone = PhoneNumber.Create(command.PhoneNumber);
        
        var identity = CitizenIdentity.Create(identityId, citizenId, "stamp", targetPhone);
        
        var oldOtp = OtpVerification.Create(identityId, Guid.NewGuid(), "oldhash", targetPhone, DateTime.UtcNow.AddMinutes(3));

        _citizenIdentityRepoMock.Setup(x => x.GetByIdAsync(identityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(identity);

        _otpVerificationRepoMock.Setup(x => x.GetPendingByCitizenIdentityIdAsync(identityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(oldOtp);

        _otpGeneratorMock.Setup(x => x.Generate(It.IsAny<int>())).Returns("123456");
        _hashProviderMock.Setup(x => x.Hash("123456")).Returns("newhash");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        
        // Assert Revoked
        Assert.Equal(OtpStatus.Cancelled, oldOtp.Status);
        _otpVerificationRepoMock.Verify(x => x.Update(oldOtp), Times.Once);

        // Assert New Created
        _otpVerificationRepoMock.Verify(x => x.Add(It.Is<OtpVerification>(o => o.CodeHash == "newhash" && o.Status == OtpStatus.Pending)), Times.Once);

        // Assert SaveChanges
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        // Assert Notification Sent
        _notificationServiceMock.Verify(x => x.SendOtpAsync("0901234567", "123456", "Activation", It.IsAny<CancellationToken>()), Times.Once);
    }
}
