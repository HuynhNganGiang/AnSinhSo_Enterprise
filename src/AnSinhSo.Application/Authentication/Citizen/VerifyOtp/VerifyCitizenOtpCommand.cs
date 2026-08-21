using System;
using AnSinhSo.Application.Authentication;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authentication.Citizen.VerifyOtp;

public sealed record VerifyCitizenOtpCommand(
    Guid RequestId,
    string OtpCode,
    string DeviceName,
    string Browser,
    string OS,
    string Platform,
    string IpAddress,
    string Fingerprint,
    string TimeZone,
    bool RememberMe
) : IRequest<Result<VerifyCitizenOtpResponse>>;

public sealed record VerifyCitizenOtpResponse(
    bool IsSuccess,
    bool RequiresRegistration,
    AuthenticationResult? Tokens = null
);
