using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Application.Common.Security;
using AnSinhSo.Contracts.Authentication;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.UserAggregate;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate;
using System;

namespace AnSinhSo.Application.Authentication.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;
    private readonly IClientInfoProvider _clientInfoProvider;
    private readonly IUserSessionRepository _userSessionRepository;

    // Use a static dummy hash to avoid generating a new hash just to verify against it,
    // which would still be fast compared to verifying a real hash.
    // However, BCrypt's Verify needs a valid hash string format.
    // In a real app, this should be a valid hash of a known long string.
    private const string DummyHash = "$2a$11$F658Q2R8qT4H.mG44p/K..pE/u45Z4a.f4m30I/c4lV3W2b4/W/M.";

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider,
        IRefreshTokenGenerator refreshTokenGenerator,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider,
        IClientInfoProvider clientInfoProvider,
        IUserSessionRepository userSessionRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _refreshTokenGenerator = refreshTokenGenerator;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
        _clientInfoProvider = clientInfoProvider;
        _userSessionRepository = userSessionRepository;
    }

    public async Task<Result<TokenResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Tìm User theo Username hoặc Email
        var user = await _userRepository.GetByEmailAsync(request.UsernameOrEmail, cancellationToken)
                   ?? await _userRepository.GetByUsernameAsync(request.UsernameOrEmail, cancellationToken);

        // 2. Dummy Hash nếu User không tồn tại (chống Timing Attack)
        if (user is null)
        {
            _passwordHasher.Verify(request.Password, DummyHash);
            return Result.Failure<TokenResponseDto>(
                Error.Failure("Auth.InvalidCredentials", "Username/Email hoặc mật khẩu không chính xác."));
        }

        // 3. Verify Password
        bool isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return Result.Failure<TokenResponseDto>(
                Error.Failure("Auth.InvalidCredentials", "Username/Email hoặc mật khẩu không chính xác."));
        }

        // 4. NeedsRehash & ChangePasswordHash nếu cần
        if (_passwordHasher.NeedsRehash(user.PasswordHash))
        {
            string newHash = _passwordHasher.Hash(request.Password);
            user.ChangePasswordHash(newHash);
        }

        // 5. Generate Access Token
        var tokenResult = _jwtProvider.Generate(user);
        string accessToken = tokenResult.AccessToken;

        // 6. Generate Refresh Token
        string plainRefreshToken = _refreshTokenGenerator.Generate();

        // 7. Hash Refresh Token
        string refreshTokenHash = _passwordHasher.Hash(plainRefreshToken);

        // 8. UserSession.Issue
        var userSession = UserSession.Issue(
            userId: user.Id.Value,
            tokenHash: refreshTokenHash,
            expiresAtUtc: _timeProvider.GetUtcNow().UtcDateTime.AddDays(tokenResult.RefreshTokenDays),
            securityStamp: user.SecurityStamp,
            ipAddress: _clientInfoProvider.IpAddress,
            deviceName: _clientInfoProvider.DeviceName,
            userAgent: _clientInfoProvider.UserAgent,
            now: _timeProvider.GetUtcNow().UtcDateTime
        );
        _userSessionRepository.Add(userSession);

        _userRepository.Update(user);

        // 9. SaveChangesAsync
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Return Result
        int expiresInSeconds = (int)(tokenResult.ExpiresAtUtc - _timeProvider.GetUtcNow().UtcDateTime).TotalSeconds;

        return Result.Success(new TokenResponseDto(
            AccessToken: accessToken,
            RefreshToken: plainRefreshToken, // Return plain token to client ONCE
            ExpiresAtUtc: tokenResult.ExpiresAtUtc,
            ExpiresInSeconds: expiresInSeconds > 0 ? expiresInSeconds : 0,
            TokenType: "Bearer"
        ));
    }
}
