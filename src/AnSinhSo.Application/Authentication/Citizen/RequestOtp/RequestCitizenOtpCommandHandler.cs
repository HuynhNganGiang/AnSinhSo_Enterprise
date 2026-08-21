using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AnSinhSo.Application.Authentication.Citizen.RequestOtp;

public sealed class RequestCitizenOtpCommandHandler : IRequestHandler<RequestCitizenOtpCommand, Result<Guid>>
{
    private readonly ICitizenIdentityRepository _identityRepository;
    private readonly IOtpVerificationService _otpVerificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RequestCitizenOtpCommandHandler> _logger;

    public RequestCitizenOtpCommandHandler(
        ICitizenIdentityRepository identityRepository,
        IOtpVerificationService otpVerificationService,
        IUnitOfWork unitOfWork,
        ILogger<RequestCitizenOtpCommandHandler> logger)
    {
        _identityRepository = identityRepository;
        _otpVerificationService = otpVerificationService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(RequestCitizenOtpCommand request, CancellationToken cancellationToken)
    {
        PhoneNumber targetPhone;
        try
        {
            targetPhone = PhoneNumber.Create(request.PhoneNumber);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<Guid>(Error.Validation("PhoneNumber.Invalid", ex.Message));
        }

        var identity = await _identityRepository.GetByPhoneNumberAsync(targetPhone, cancellationToken);
        if (identity == null)
        {
            _logger.LogWarning("OTP requested for unregistered phone number: {PhoneNumber}", request.PhoneNumber);
            return Result.Failure<Guid>(Error.NotFound("CitizenIdentity.NotFound", "No citizen identity found for this phone number."));
        }

        var requestId = Guid.NewGuid();
        var result = await _otpVerificationService.GenerateAndSendOtpAsync(identity.Id, request.PhoneNumber, requestId, cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(requestId);
    }
}
