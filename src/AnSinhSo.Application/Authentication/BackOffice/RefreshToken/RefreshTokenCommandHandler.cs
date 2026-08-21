using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Application.Abstractions.Security;
using AnSinhSo.Domain.Aggregates.SecurityAggregate;
using AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;
using AnSinhSo.Domain.Aggregates.UserAggregate;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;
using Microsoft.Extensions.Options;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate.ValueObjects;
namespace AnSinhSo.Application.Authentication.BackOffice.RefreshToken;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthenticationResult>>
{
    private readonly IUserSessionRepository _userSessionRepository;
    private readonly ICitizenIdentityRepository _citizenIdentityRepository;
    private readonly IHashProvider _hashProvider;
    private readonly IJwtProvider _jwtProvider;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AuthenticationOptions _options;

    public RefreshTokenCommandHandler(
        IUserSessionRepository userSessionRepository,
        ICitizenIdentityRepository citizenIdentityRepository,
        IHashProvider hashProvider,
        IJwtProvider jwtProvider,
        ITokenGenerator tokenGenerator,
        IUnitOfWork unitOfWork,
        IOptions<AuthenticationOptions> options)
    {
        _userSessionRepository = userSessionRepository;
        _citizenIdentityRepository = citizenIdentityRepository;
        _hashProvider = hashProvider;
        _jwtProvider = jwtProvider;
        _tokenGenerator = tokenGenerator;
        _unitOfWork = unitOfWork;
        _options = options.Value;
    }

    public async Task<Result<AuthenticationResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var hash = _hashProvider.Hash(request.RefreshToken);

        var session = await _userSessionRepository.GetByRefreshTokenHashAsync(hash, cancellationToken);

        if (session is null)
        {
            return Result.Failure<AuthenticationResult>(Error.Failure("Auth.InvalidToken", "Token không hợp lệ."));
        }

        if (session.IsRevoked)
        {
            // Replay Attack Detection
            var familySessions = await _userSessionRepository.GetFamilySessionsAsync(session.RefreshTokenFamilyId, cancellationToken);
            foreach (var s in familySessions)
            {
                s.Revoke("Compromised: Token Replay");
                _userSessionRepository.Update(s);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Failure<AuthenticationResult>(SessionErrors.Compromised);
        }

        if (session.IsExpired())
        {
            return Result.Failure<AuthenticationResult>(SessionErrors.Expired);
        }

        if (!session.IsActive())
        {
            return Result.Failure<AuthenticationResult>(Error.Failure("Auth.InvalidSession", "Phiên đăng nhập không hợp lệ hoặc đã bị thu hồi."));
        }

        var identity = await _citizenIdentityRepository.GetByIdAsync(new CitizenIdentityId(session.CitizenIdentityId), cancellationToken);
        if (identity is null || identity.Status != AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations.IdentityStatus.Verified)
        {
            return Result.Failure<AuthenticationResult>(Error.Failure("Auth.InvalidUser", "Tài khoản không hợp lệ hoặc chưa xác thực."));
        }

        // Generate new Refresh Token
        var newRawRefreshToken = _tokenGenerator.GenerateRefreshToken();
        var newHashedRefreshToken = _hashProvider.Hash(newRawRefreshToken);
        
        var refreshTokenExpiryDays = _options.RefreshTokenLifetimeDays;
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        session.RotateRefreshToken(newHashedRefreshToken, refreshTokenExpiry);
        _userSessionRepository.Update(session);

        // Generate new JWT
        var jwtExpiryMinutes = _options.AccessTokenLifetimeMinutes;
        var accessToken = _jwtProvider.GenerateAccessToken(identity, session.Id);

        // Commit transaction
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = new AuthenticationResult(
            accessToken,
            newRawRefreshToken,
            jwtExpiryMinutes * 60,
            identity.Id.Value);

        return Result.Success(result);
    }
}
