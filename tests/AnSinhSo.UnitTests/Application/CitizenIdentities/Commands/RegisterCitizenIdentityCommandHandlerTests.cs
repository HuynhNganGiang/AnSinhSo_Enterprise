using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.CitizenIdentities.Commands.RegisterCitizenIdentity;
using AnSinhSo.Application.Common.Interfaces.Security;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.ValueObjects;
using Moq;
using Xunit;

namespace AnSinhSo.UnitTests.Application.CitizenIdentities.Commands;

public class RegisterCitizenIdentityCommandHandlerTests
{
    private readonly Mock<ICitizenRepository> _citizenRepositoryMock;
    private readonly Mock<ICitizenIdentityRepository> _citizenIdentityRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ISecurityStampGenerator> _securityStampGeneratorMock;
    private readonly RegisterCitizenIdentityCommandHandler _handler;

    public RegisterCitizenIdentityCommandHandlerTests()
    {
        _citizenRepositoryMock = new Mock<ICitizenRepository>();
        _citizenIdentityRepositoryMock = new Mock<ICitizenIdentityRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _securityStampGeneratorMock = new Mock<ISecurityStampGenerator>();

        _handler = new RegisterCitizenIdentityCommandHandler(
            _citizenRepositoryMock.Object,
            _citizenIdentityRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _securityStampGeneratorMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenCitizenNotFound_ShouldReturnFailure()
    {
        // Arrange
        var command = new RegisterCitizenIdentityCommand(Guid.NewGuid(), "0912345678");
        _citizenRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<CitizenId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Citizen?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(IdentityErrors.CitizenNotFound, result.Error);
        _citizenIdentityRepositoryMock.Verify(x => x.Add(It.IsAny<CitizenIdentity>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenIdentityAlreadyExists_ShouldReturnFailureIdempotent()
    {
        // Arrange
        var command = new RegisterCitizenIdentityCommand(Guid.NewGuid(), "0912345678");
        var citizenResult = Citizen.Create(
            new CitizenId(command.CitizenId), 
            FullName.Create("Nguyen", "Van", "A").Value, 
            CitizenNumber.Create("001090000001").Value, 
            new DateTime(1990, 1, 1), 
            AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations.Gender.Male, 
            PhoneNumber.Create("0912345678").Value, 
            Address.Create("123", "Phuong", "Quan", "Thanh Pho", PostalCode.Create("700000").Value).Value, 
            Email.Create("test@test.com").Value);
        var citizen = citizenResult.Value;
            
        _citizenRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<CitizenId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Citizen?)citizen);

        var existingIdentity = CitizenIdentity.Create(CitizenIdentityId.Create(Guid.NewGuid()), new CitizenId(command.CitizenId), "stamp");

        _citizenIdentityRepositoryMock
            .Setup(x => x.GetByCitizenIdAsync(It.IsAny<CitizenId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingIdentity);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(IdentityErrors.IdentityAlreadyExists, result.Error);
        _citizenIdentityRepositoryMock.Verify(x => x.Add(It.IsAny<CitizenIdentity>()), Times.Never); // AD #30: Idempotent check
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenValid_ShouldProvisionAndReturnSuccess()
    {
        // Arrange
        var command = new RegisterCitizenIdentityCommand(Guid.NewGuid(), "0912345678");
        var citizenResult = Citizen.Create(
            new CitizenId(command.CitizenId), 
            FullName.Create("Nguyen", "Van", "A").Value, 
            CitizenNumber.Create("001090000001").Value, 
            new DateTime(1990, 1, 1), 
            AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations.Gender.Male, 
            PhoneNumber.Create("0912345678").Value, 
            Address.Create("123", "Phuong", "Quan", "Thanh Pho", PostalCode.Create("700000").Value).Value, 
            Email.Create("test@test.com").Value);
        var citizen = citizenResult.Value;

        _citizenRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<CitizenId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Citizen?)citizen);

        _citizenIdentityRepositoryMock
            .Setup(x => x.GetByCitizenIdAsync(It.IsAny<CitizenId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CitizenIdentity?)null);

        _securityStampGeneratorMock
            .Setup(x => x.Generate())
            .Returns("new-stamp-123");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        
        _citizenIdentityRepositoryMock.Verify(x => x.Add(It.Is<CitizenIdentity>(i => 
            i.CitizenId.Value == command.CitizenId && 
            i.SecurityStamp == "new-stamp-123" &&
            i.Status == AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations.IdentityStatus.Pending &&
            i.PrimaryPhone != null && i.PrimaryPhone.Value == command.PhoneNumber
        )), Times.Once); // AD #32: Init with phone number, Status is PendingVerification
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once); // AD #34: Event deferred
    }
}
