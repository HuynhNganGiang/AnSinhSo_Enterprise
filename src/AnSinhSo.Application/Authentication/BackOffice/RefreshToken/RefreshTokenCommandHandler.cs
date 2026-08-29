using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Application.Abstractions.Security;
using AnSinhSo.Application.Authentication.BackOffice.RefreshToken.Resolvers;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;

using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;
using Microsoft.Extensions.Options;

namespace AnSinhSo.Application.Authentication.BackOffice.RefreshToken;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthenticationResult>>
{
    private readonly IRefreshSessionResolver _sessionResolver;
    private readonly ICitizenIdentityRepository _citizenIdentityRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHashProvider _hashProvider;
    private readonly IJwtProvider _jwtProvider;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AuthenticationOptions _options;

    public RefreshTokenCommandHandler(
        IRefreshSessionResolver sessionResolver,
        ICitizenIdentityRepository citizenIdentityRepository,
        IUserRepository userRepository,
        IHashProvider hashProvider,
        IJwtProvider jwtProvider,
        ITokenGenerator tokenGenerator,
        IUnitOfWork unitOfWork,
        IOptions<AuthenticationOptions> options)
    {
        _sessionResolver = sessionResolver;
        _citizenIdentityRepository = citizenIdentityRepository;
        _userRepository = userRepository;
        _hashProvider = hashProvider;
        _jwtProvider = jwtProvider;
        _tokenGenerator = tokenGenerator;
        _unitOfWork = unitOfWork;
        _options = options.Value;
    }

    public async Task<Result<AuthenticationResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // 1. Resolve Session Context (Handles parsing, replay detection, revocation, and expiration)
        var hash = _hashProvider.Hash(request.RefreshToken);
        var contextResult = await _sessionResolver.ResolveAsync(hash, cancellationToken);

        if (contextResult.IsFailure)
        {
            if (contextResult.Error == SessionErrors.Compromised)
            {
                // If resolver caught a replay attack, commit the revocations it performed.
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            return Result.Failure<AuthenticationResult>(contextResult.Error);
        }

        var context = contextResult.Value;

        // 2. Validate Identity
        AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.CitizenIdentity? identity = null;
        if (context.CitizenIdentityId.HasValue)
        {
            identity = await _citizenIdentityRepository.GetByIdAsync(new CitizenIdentityId(context.CitizenIdentityId.Value), cancellationToken);
            if (identity is null || identity.Status != AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations.IdentityStatus.Verified)
            {
                return Result.Failure<AuthenticationResult>(Error.Failure("Auth.InvalidUser", "Tài khoản công dân không hợp lệ hoặc chưa xác thực."));
            }
        }

        AnSinhSo.Domain.Aggregates.UserAggregate.User? user = null;
        if (context.UserId.HasValue)
        {
            user = await _userRepository.GetByIdAsync(new AnSinhSo.Domain.Aggregates.UserAggregate.UserId(context.UserId.Value), cancellationToken);
            if (user == null)
            {
                return Result.Failure<AuthenticationResult>(Error.Failure("Auth.InvalidUser", "Tài khoản User không tồn tại."));
            }
        }

        if (identity == null && user == null)
        {
             return Result.Failure<AuthenticationResult>(Error.Failure("Auth.InvalidUser", "Phiên đăng nhập không gắn với danh tính hợp lệ."));
        }

        // 3. Issue New Tokens (SRP: Handlers/Generators issue tokens)
        var newRawRefreshToken = _tokenGenerator.GenerateRefreshToken();
        var newHashedRefreshToken = _hashProvider.Hash(newRawRefreshToken);
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(_options.RefreshTokenLifetimeDays);

        var jwtExpiryMinutes = _options.AccessTokenLifetimeMinutes;

        // 4. Rotate Session
        int maxRetries = 2;
        string accessToken = string.Empty;

        for (int retry = 0; retry <= maxRetries; retry++)
        {
            try
            {
                await _sessionResolver.RotateAsync(context, newHashedRefreshToken, refreshTokenExpiry, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (identity != null)
                {
                    accessToken = _jwtProvider.GenerateAccessToken(identity, new DeviceSessionId(context.SessionId));
                }
                else
                {
                    accessToken = _jwtProvider.GenerateAccessTokenForUser(user!, new DeviceSessionId(context.SessionId));
                }
                break;
            }
            catch (AnSinhSo.Domain.Exceptions.ConcurrencyException)
            {
                if (retry == maxRetries)
                {
                    return Result.Failure<AuthenticationResult>(Error.Conflict("Auth.Concurrency", "Xung đột dữ liệu. Vui lòng thử lại."));
                }

                _unitOfWork.ClearChangeTracker();
                // Loop continues and RotateAsync is called again to fetch fresh state and apply changes cleanly
            }
        }

        var result = new AuthenticationResult(
            accessToken,
            newRawRefreshToken,
            jwtExpiryMinutes * 60,
            identity?.Id.Value ?? user!.Id.Value);

        return Result.Success(result);
    }
}
