using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Security;
using AnSinhSo.Application.OtpVerifications.Commands.VerifyOtp;
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

public class VerifyOtpCommandHandlerTests
{
    private readonly Mock<IOtpVerificationRepository> _otpVerificationRepoMock;
    private readonly Mock<ICitizenIdentityRepository> _citizenIdentityRepoMock;
    private readonly Mock<IHashProvider> _hashProviderMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly VerifyOtpCommandHandler _handler;

    public VerifyOtpCommandHandlerTests()
    {
        _otpVerificationRepoMock = new Mock<IOtpVerificationRepository>();
        _citizenIdentityRepoMock = new Mock<ICitizenIdentityRepository>();
        _hashProviderMock = new Mock<IHashProvider>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new VerifyOtpCommandHandler(
            _otpVerificationRepoMock.Object,
            _citizenIdentityRepoMock.Object,
            _hashProviderMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenOtpNotFound()
    {
        // Arrange
        var command = new VerifyOtpCommand(Guid.NewGuid(), "123456");

        _otpVerificationRepoMock.Setup(x => x.GetPendingByCitizenIdentityIdAsync(It.IsAny<CitizenIdentityId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((OtpVerification?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(OtpErrors.NotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenIdentityNotFound()
    {
        // Arrange
        var command = new VerifyOtpCommand(Guid.NewGuid(), "123456");
        var identityId = CitizenIdentityId.Create(command.CitizenIdentityId);
        var targetPhone = PhoneNumber.Create("0901234567");

        var otp = OtpVerification.Create(identityId, Guid.NewGuid(), "hash", targetPhone, DateTime.UtcNow.AddMinutes(3));

        _otpVerificationRepoMock.Setup(x => x.GetPendingByCitizenIdentityIdAsync(identityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(otp);

        _citizenIdentityRepoMock.Setup(x => x.GetByIdAsync(identityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CitizenIdentity?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(IdentityErrors.IdentityNotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenOtpIsInvalid()
    {
        // Arrange
        var command = new VerifyOtpCommand(Guid.NewGuid(), "111111");
        var identityId = CitizenIdentityId.Create(command.CitizenIdentityId);
        var targetPhone = PhoneNumber.Create("0901234567");

        var otp = OtpVerification.Create(identityId, Guid.NewGuid(), "validHash", targetPhone, DateTime.UtcNow.AddMinutes(3));
        
        var citizenId = new CitizenId(Guid.NewGuid());
        var identity = CitizenIdentity.Create(identityId, citizenId, "stamp", targetPhone);

        _otpVerificationRepoMock.Setup(x => x.GetPendingByCitizenIdentityIdAsync(identityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(otp);

        _citizenIdentityRepoMock.Setup(x => x.GetByIdAsync(identityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(identity);

        _hashProviderMock.Setup(x => x.Hash("111111")).Returns("invalidHash");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(OtpErrors.Invalid.Code, result.Error.Code);
        
        // Ensure failed attempt was incremented and updated
        Assert.Equal(1, otp.FailedAttemptCount);
        _otpVerificationRepoMock.Verify(x => x.Update(otp), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenOtpIsValid()
    {
        // Arrange
        var command = new VerifyOtpCommand(Guid.NewGuid(), "123456");
        var identityId = CitizenIdentityId.Create(command.CitizenIdentityId);
        var targetPhone = PhoneNumber.Create("0901234567");

        var otp = OtpVerification.Create(identityId, Guid.NewGuid(), "validHash", targetPhone, DateTime.UtcNow.AddMinutes(3));
        
        var citizenId = new CitizenId(Guid.NewGuid());
        var identity = CitizenIdentity.Create(identityId, citizenId, "stamp", targetPhone);

        _otpVerificationRepoMock.Setup(x => x.GetPendingByCitizenIdentityIdAsync(identityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(otp);

        _citizenIdentityRepoMock.Setup(x => x.GetByIdAsync(identityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(identity);

        _hashProviderMock.Setup(x => x.Hash("123456")).Returns("validHash");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        
        // Assert Orchestration
        Assert.Equal(OtpStatus.Verified, otp.Status);
        Assert.Equal(IdentityStatus.Verified, identity.Status);

        _otpVerificationRepoMock.Verify(x => x.Update(otp), Times.Once);
        _citizenIdentityRepoMock.Verify(x => x.Update(identity), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
