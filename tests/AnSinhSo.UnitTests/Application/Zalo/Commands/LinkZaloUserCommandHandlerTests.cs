using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Zalo.Commands.LinkZaloUser;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.ZaloUserAggregate;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace AnSinhSo.UnitTests.Application.Zalo.Commands;

public class LinkZaloUserCommandHandlerTests
{
    private readonly Mock<IZaloUserRepository> _zaloUserRepositoryMock;
    private readonly Mock<ICitizenIdentityRepository> _citizenIdentityRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly LinkZaloUserCommandHandler _handler;

    public LinkZaloUserCommandHandlerTests()
    {
        _zaloUserRepositoryMock = new Mock<IZaloUserRepository>();
        _citizenIdentityRepositoryMock = new Mock<ICitizenIdentityRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new LinkZaloUserCommandHandler(
            _zaloUserRepositoryMock.Object,
            _citizenIdentityRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenZaloUserNotFound()
    {
        // Arrange
        var command = new LinkZaloUserCommand("zalo_id", Guid.NewGuid());
        
        _zaloUserRepositoryMock.Setup(x => x.GetByZaloIdAsync(command.ZaloUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ZaloUser?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Zalo.NotFound", result.Error.Code);
    }
}
