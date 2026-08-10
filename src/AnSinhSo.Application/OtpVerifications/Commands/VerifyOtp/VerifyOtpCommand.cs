using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.OtpVerifications.Commands.VerifyOtp;

public sealed record VerifyOtpCommand(
    Guid CitizenIdentityId,
    string OtpCode
) : IRequest<Result>;
