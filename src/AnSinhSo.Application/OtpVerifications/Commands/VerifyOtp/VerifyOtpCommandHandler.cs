using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Security;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.OtpVerifications.Commands.VerifyOtp;

public sealed class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, Result>
{
    private readonly IOtpVerificationRepository _otpVerificationRepository;
    private readonly ICitizenIdentityRepository _citizenIdentityRepository;
    private readonly IHashProvider _hashProvider;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyOtpCommandHandler(
        IOtpVerificationRepository otpVerificationRepository,
        ICitizenIdentityRepository citizenIdentityRepository,
        IHashProvider hashProvider,
        IUnitOfWork unitOfWork)
    {
        _otpVerificationRepository = otpVerificationRepository;
        _citizenIdentityRepository = citizenIdentityRepository;
        _hashProvider = hashProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        var identityId = CitizenIdentityId.Create(request.CitizenIdentityId);

        // 1. Get Pending OTP
        var pendingOtp = await _otpVerificationRepository.GetPendingByCitizenIdentityIdAsync(identityId, cancellationToken);
        if (pendingOtp is null)
        {
            return Result.Failure(OtpErrors.NotFound);
        }

        // 2. Get CitizenIdentity
        var identity = await _citizenIdentityRepository.GetByIdAsync(identityId, cancellationToken);
        if (identity is null)
        {
            return Result.Failure(IdentityErrors.IdentityNotFound);
        }

        // 3. Hash input code
        var hashedCode = _hashProvider.Hash(request.OtpCode);

        try
        {
            // 4. Verify (Domain Logic)
            pendingOtp.Verify(hashedCode, maxAttempts: 5, DateTime.UtcNow);

            // 5. Cross Aggregate Orchestration (AD #54) - Activate Identity (AD #67, AD #53)
            identity.VerifyPhoneNumber(pendingOtp.TargetPhone, DateTime.UtcNow);

            // 6. Single Transaction (AD #65)
            _otpVerificationRepository.Update(pendingOtp);
            _citizenIdentityRepository.Update(identity);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            // Domain Exception to Result mapping
            _otpVerificationRepository.Update(pendingOtp);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (ex.Message.Contains("expired", StringComparison.OrdinalIgnoreCase))
            {
                return Result.Failure(OtpErrors.Expired);
            }
            if (ex.Message.Contains("revoked", StringComparison.OrdinalIgnoreCase))
            {
                return Result.Failure(OtpErrors.ExceedMaxAttempts);
            }
            if (ex.Message.Contains("already been used", StringComparison.OrdinalIgnoreCase))
            {
                return Result.Failure(OtpErrors.AlreadyUsed);
            }
            if (ex.Message.Contains("Invalid OTP code", StringComparison.OrdinalIgnoreCase))
            {
                return Result.Failure(OtpErrors.Invalid);
            }

            return Result.Failure(Error.Validation("Otp.VerificationFailed", ex.Message));
        }
    }
}
