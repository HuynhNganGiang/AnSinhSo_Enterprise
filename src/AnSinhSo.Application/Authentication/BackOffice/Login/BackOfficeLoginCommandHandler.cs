using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authentication;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;
using System.Threading.Tasks;
using System.Linq;

namespace AnSinhSo.Application.Authentication.BackOffice.BackOfficeLogin;

public sealed class BackOfficeLoginCommandHandler : IRequestHandler<BackOfficeLoginCommand, Result<AuthenticationResult>>
{
    private readonly AnSinhSo.Domain.Interfaces.IUserRepository _userRepository;
    private readonly AnSinhSo.Domain.Interfaces.ISecurityRepository _securityRepository;
    private readonly AnSinhSo.Application.Abstractions.Security.IHashProvider _hashProvider;
    private readonly AnSinhSo.Application.Common.Security.IPasswordHasher _passwordHasher;
    private readonly AnSinhSo.Application.Abstractions.Authentication.IJwtProvider _jwtProvider;
    private readonly AnSinhSo.Application.Abstractions.Authentication.ITokenGenerator _tokenGenerator;
    private readonly AnSinhSo.Domain.Interfaces.IUnitOfWork _unitOfWork;
    private readonly Microsoft.Extensions.Options.IOptions<AuthenticationOptions> _authOptions;

    public BackOfficeLoginCommandHandler(
        AnSinhSo.Domain.Interfaces.IUserRepository userRepository,
        AnSinhSo.Domain.Interfaces.ISecurityRepository securityRepository,
        AnSinhSo.Application.Abstractions.Security.IHashProvider hashProvider,
        AnSinhSo.Application.Common.Security.IPasswordHasher passwordHasher,
        AnSinhSo.Application.Abstractions.Authentication.IJwtProvider jwtProvider,
        AnSinhSo.Application.Abstractions.Authentication.ITokenGenerator tokenGenerator,
        AnSinhSo.Domain.Interfaces.IUnitOfWork unitOfWork,
        Microsoft.Extensions.Options.IOptions<AuthenticationOptions> authOptions)
    {
        _userRepository = userRepository;
        _securityRepository = securityRepository;
        _hashProvider = hashProvider;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _tokenGenerator = tokenGenerator;
        _unitOfWork = unitOfWork;
        _authOptions = authOptions;
    }

    public async Task<Result<AuthenticationResult>> Handle(BackOfficeLoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Password Complexity Check (Pre-DB check)
        var complexityResult = PasswordPolicyValidator.Validate(request.Password, _authOptions.Value);
        if (complexityResult.IsFailure)
        {
            // Optional: You could log this attempt. For now we just return the failure.
            return Result.Failure<AuthenticationResult>(complexityResult.Error);
        }

        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);

        if (user == null)
        {
            return Result.Failure<AuthenticationResult>(Error.Failure("Auth.InvalidCredentials", "Tên đăng nhập hoặc mật khẩu không chính xác."));
        }

        if (user.IsLocked)
        {
            _securityRepository.AddAuditLogin(AnSinhSo.Domain.Aggregates.SecurityAggregate.AuditLogin.CreateFailure(
                user.Id.Value, request.IpAddress, request.UserAgent, request.TimeZone, "Tài khoản đang bị khóa."));
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Failure<AuthenticationResult>(Error.Failure("Auth.AccountLocked", "Tài khoản đang bị khóa. Vui lòng thử lại sau."));
        }

        bool isPasswordValid = false;
        try
        {
            isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
        }
        catch (System.Exception)
        {
            // Fallback for legacy SHA256 hashes if BCrypt fails to parse
            isPasswordValid = _hashProvider.Verify(request.Password, user.PasswordHash);

            // If valid, we should upgrade the hash here to BCrypt
            if (isPasswordValid)
            {
                user.ChangePasswordHash(_passwordHasher.Hash(request.Password));
                _userRepository.Update(user);
            }
        }

        if (!isPasswordValid)
        {
            user.RecordAccessFailed(5, TimeSpan.FromMinutes(30)); // 5 attempts, 30 min lockout
            _userRepository.Update(user);

            _securityRepository.AddAuditLogin(AnSinhSo.Domain.Aggregates.SecurityAggregate.AuditLogin.CreateFailure(
                user.Id.Value, request.IpAddress, request.UserAgent, request.TimeZone, "Sai mật khẩu."));

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Failure<AuthenticationResult>(Error.Failure("Auth.InvalidCredentials", "Tên đăng nhập hoặc mật khẩu không chính xác."));
        }

        // Success Login
        user.ResetAccessFailedCount();
        _userRepository.Update(user);

        _securityRepository.AddLoginHistory(AnSinhSo.Domain.Aggregates.SecurityAggregate.LoginHistory.RecordLogin(
            user.Id.Value, request.IpAddress, request.DeviceName, "Unknown Location"));

        _securityRepository.AddAuditLogin(AnSinhSo.Domain.Aggregates.SecurityAggregate.AuditLogin.CreateSuccess(
            user.Id.Value, request.IpAddress, request.UserAgent, request.TimeZone));

        _securityRepository.AddSecurityLog(AnSinhSo.Domain.Aggregates.SecurityAggregate.SecurityLog.Create(
            user.Id.Value, AnSinhSo.Domain.Aggregates.SecurityAggregate.Enumerations.SecurityEventType.LOGIN_SUCCESS, "Đăng nhập Back Office thành công.", request.IpAddress));

        int maxRetries = 2;
        string jwtToken = string.Empty;
        string finalRefreshTokenString = string.Empty;

        for (int retry = 0; retry <= maxRetries; retry++)
        {
            try
            {
                var activeSessions = await _securityRepository.GetActiveDeviceSessionsByUserIdAsync(user.Id.Value, cancellationToken);
                if (activeSessions.Count >= 5)
                {
                    var sessionsToRevoke = activeSessions.OrderBy(s => s.LastSeenAt).Take(activeSessions.Count - 4);
                    foreach (var s in sessionsToRevoke)
                    {
                        s.Revoke("Device limit exceeded (BackOffice Login)");
                        _securityRepository.UpdateDeviceSession(s);
                    }
                }

                var deviceSession = AnSinhSo.Domain.Aggregates.SecurityAggregate.DeviceSession.Create(
                    user.Id.Value, request.DeviceName, "UnknownBrowser", "UnknownOS", "UnknownPlatform", request.IpAddress, "", request.RememberMe, user.SecurityStamp);

                finalRefreshTokenString = _tokenGenerator.GenerateRefreshToken();
                var hashedRefreshToken = _hashProvider.Hash(finalRefreshTokenString);
                var expirationDays = request.RememberMe ? 30 : 1;
                var refreshToken = AnSinhSo.Domain.Aggregates.SecurityAggregate.RefreshToken.Create(
                    user.Id.Value, null, hashedRefreshToken, Guid.NewGuid(), DateTime.UtcNow.AddDays(expirationDays), deviceSession.Id);

                deviceSession.LinkRefreshToken(refreshToken.Id.Value);

                _securityRepository.AddDeviceSession(deviceSession);
                _securityRepository.AddRefreshToken(refreshToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                jwtToken = _jwtProvider.GenerateAccessTokenForUser(user, deviceSession.Id);
                break;
            }
            catch (AnSinhSo.Domain.Exceptions.ConcurrencyException)
            {
                if (retry == maxRetries)
                {
                    return Result.Failure<AuthenticationResult>(Error.Conflict("Auth.Concurrency", "Xung đột dữ liệu khi đăng nhập. Vui lòng thử lại."));
                }

                _unitOfWork.ClearChangeTracker();
            }
        }

        return Result.Success(new AuthenticationResult(
            jwtToken,
            finalRefreshTokenString,
            request.RememberMe ? 30 * 24 * 3600 : 24 * 3600, // Expiration time in seconds
            user.Id.Value
        ));
    }
}
