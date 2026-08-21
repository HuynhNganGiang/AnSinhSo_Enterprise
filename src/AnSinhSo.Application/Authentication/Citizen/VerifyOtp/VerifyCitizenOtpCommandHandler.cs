using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System;

namespace AnSinhSo.Application.Authentication.Citizen.VerifyOtp;

public sealed class VerifyCitizenOtpCommandHandler : IRequestHandler<VerifyCitizenOtpCommand, Result<VerifyCitizenOtpResponse>>
{
    private readonly IOtpVerificationService _otpService;
    private readonly IIdentityVerificationService _identityService;
    private readonly ITokenIssuingService _tokenService;
    private readonly ISecurityAuditService _auditService;
    private readonly IOtpVerificationRepository _otpRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<VerifyCitizenOtpCommandHandler> _logger;

    public VerifyCitizenOtpCommandHandler(
        IOtpVerificationService otpService,
        IIdentityVerificationService identityService,
        ITokenIssuingService tokenService,
        ISecurityAuditService auditService,
        IOtpVerificationRepository otpRepository,
        IUnitOfWork unitOfWork,
        ILogger<VerifyCitizenOtpCommandHandler> logger)
    {
        _otpService = otpService;
        _identityService = identityService;
        _tokenService = tokenService;
        _auditService = auditService;
        _otpRepository = otpRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<VerifyCitizenOtpResponse>> Handle(VerifyCitizenOtpCommand request, CancellationToken cancellationToken)
    {
        // 1. Verify OTP
        var verifyResult = await _otpService.VerifyOtpAsync(request.RequestId, request.OtpCode, cancellationToken);
        if (verifyResult.IsFailure)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken); // Save the failed attempt count
            return Result.Failure<VerifyCitizenOtpResponse>(verifyResult.Error);
        }

        // We need the CitizenIdentityId. Fetch the OTP aggregate.
        var otp = await _otpRepository.GetByRequestIdAsync(request.RequestId, cancellationToken);
        if (otp == null)
        {
            return Result.Failure<VerifyCitizenOtpResponse>(Error.NotFound("Otp.NotFound", "OTP record not found."));
        }

        // 2. Update Identity Status
        var identityResult = await _identityService.MarkAsVerifiedAsync(otp.CitizenIdentityId, cancellationToken);
        if (identityResult.IsFailure)
        {
            return Result.Failure<VerifyCitizenOtpResponse>(identityResult.Error);
        }

        var identity = identityResult.Value;

        // 3. Issue Tokens (Only if User exists)
        var tokenResult = await _tokenService.IssueTokensAsync(
            identity,
            request.DeviceName,
            request.Browser,
            request.OS,
            request.Platform,
            request.IpAddress,
            request.Fingerprint,
            request.RememberMe,
            cancellationToken
        );

        if (tokenResult.IsSuccess)
        {
            await _auditService.LogSuccessAsync(
                tokenResult.Value.UserId,
                request.IpAddress,
                request.Browser,
                request.DeviceName,
                request.TimeZone,
                cancellationToken
            );
        }

        // 4. Save Changes in a Single Transaction
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (tokenResult.IsFailure && tokenResult.Error.Code == "User.NotFound")
        {
            return Result.Success(new VerifyCitizenOtpResponse(
                IsSuccess: true,
                RequiresRegistration: true,
                Tokens: null
            ));
        }

        if (tokenResult.IsFailure)
        {
            // Some other error generating tokens
            return Result.Failure<VerifyCitizenOtpResponse>(tokenResult.Error);
        }

        // Tokens issued successfully
        return Result.Success(new VerifyCitizenOtpResponse(
            IsSuccess: true,
            RequiresRegistration: false,
            Tokens: tokenResult.Value
        ));
    }
}
