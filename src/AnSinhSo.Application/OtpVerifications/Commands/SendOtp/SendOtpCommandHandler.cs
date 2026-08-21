using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Notifications;
using AnSinhSo.Application.Abstractions.Security;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.OtpVerifications.Commands.SendOtp;

public sealed class SendOtpCommandHandler : IRequestHandler<SendOtpCommand, Result>
{
    private readonly ICitizenIdentityRepository _citizenIdentityRepository;
    private readonly IOtpVerificationRepository _otpVerificationRepository;
    private readonly IOtpGenerator _otpGenerator;
    private readonly IHashProvider _hashProvider;
    private readonly IOtpNotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;

    public SendOtpCommandHandler(
        ICitizenIdentityRepository citizenIdentityRepository,
        IOtpVerificationRepository otpVerificationRepository,
        IOtpGenerator otpGenerator,
        IHashProvider hashProvider,
        IOtpNotificationService notificationService,
        IUnitOfWork unitOfWork)
    {
        _citizenIdentityRepository = citizenIdentityRepository;
        _otpVerificationRepository = otpVerificationRepository;
        _otpGenerator = otpGenerator;
        _hashProvider = hashProvider;
        _notificationService = notificationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SendOtpCommand request, CancellationToken cancellationToken)
    {
        var identityId = CitizenIdentityId.Create(request.CitizenIdentityId);

        // 1. Validate Identity
        var identity = await _citizenIdentityRepository.GetByIdAsync(identityId, cancellationToken);
        if (identity is null)
        {
            return Result.Failure(IdentityErrors.IdentityNotFound);
        }

        if (identity.Status != IdentityStatus.Pending)
        {
            // Just return success or validation error depending on business, usually InvalidOperation
            return Result.Failure(Error.Validation("Identity.NotPending", "Tài khoản không ở trạng thái chờ xác thực."));
        }

        // 2. Revoke old pending OTP
        var pendingOtp = await _otpVerificationRepository.GetPendingByCitizenIdentityIdAsync(identityId, cancellationToken);
        if (pendingOtp is not null)
        {
            pendingOtp.Revoke("Requested new OTP");
            _otpVerificationRepository.Update(pendingOtp);
        }

        // 3. Generate & Hash
        var rawOtp = _otpGenerator.Generate(6);
        var hashedOtp = _hashProvider.Hash(rawOtp);
        var targetPhone = PhoneNumber.Create(request.PhoneNumber);

        // 4. Create new OTP Aggregate
        var newOtp = OtpVerification.Create(
            identityId,
            Guid.NewGuid(),
            hashedOtp,
            targetPhone,
            DateTime.UtcNow.AddMinutes(3)
        );

        _otpVerificationRepository.Add(newOtp);

        // 5. Save Changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 6. Send Notification
        await _notificationService.SendOtpAsync(targetPhone.Value, rawOtp, request.Purpose, cancellationToken);

        return Result.Success();
    }
}
