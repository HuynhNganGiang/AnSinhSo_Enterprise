using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Security;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Application.Zalo.Commands.AuthenticateZaloUser;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.ZaloUserAggregate;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Interfaces.Repositories;
using AnSinhSo.Application.Zalo;
using Microsoft.Extensions.Options;
using AnSinhSo.Application.Authentication;
using Moq;
using Xunit;

namespace AnSinhSo.UnitTests.Application.Zalo.Commands;

public class AuthenticateZaloUserCommandHandlerTests
{
    private readonly Mock<IZaloUserRepository> _zaloUserRepositoryMock;
    private readonly Mock<ICitizenIdentityRepository> _citizenIdentityRepositoryMock;
    private readonly Mock<IZaloOAService> _zaloOAServiceMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ISecurityRepository> _securityRepositoryMock;
    private readonly Mock<IHashProvider> _hashProviderMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly Mock<ITokenGenerator> _tokenGeneratorMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IOptions<AuthenticationOptions> _options;
    private readonly AuthenticateZaloUserCommandHandler _handler;

    public AuthenticateZaloUserCommandHandlerTests()
    {
        _zaloUserRepositoryMock = new Mock<IZaloUserRepository>();
        _citizenIdentityRepositoryMock = new Mock<ICitizenIdentityRepository>();
        _zaloOAServiceMock = new Mock<IZaloOAService>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _securityRepositoryMock = new Mock<ISecurityRepository>();
        _hashProviderMock = new Mock<IHashProvider>();
        _jwtProviderMock = new Mock<IJwtProvider>();
        _tokenGeneratorMock = new Mock<ITokenGenerator>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _options = Options.Create(new AuthenticationOptions());

        _handler = new AuthenticateZaloUserCommandHandler(
            _zaloOAServiceMock.Object,
            _zaloUserRepositoryMock.Object,
            _citizenIdentityRepositoryMock.Object,
            _userRepositoryMock.Object,
            _securityRepositoryMock.Object,
            _hashProviderMock.Object,
            _jwtProviderMock.Object,
            _tokenGeneratorMock.Object,
            _unitOfWorkMock.Object,
            _options);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenZaloOAServiceReturnsNull()
    {
        // Arrange
        var command = new AuthenticateZaloUserCommand("invalid_code", "127.0.0.1", "userAgent", "deviceName");
        
        _zaloOAServiceMock.Setup(x => x.GetAccessTokenAsync(command.AuthorizationCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(string.Empty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Zalo.AuthFailed", result.Error.Code);
    }
}
