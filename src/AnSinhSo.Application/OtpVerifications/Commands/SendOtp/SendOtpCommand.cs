using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.OtpVerifications.Commands.SendOtp;

public sealed record SendOtpCommand(
    Guid CitizenIdentityId,
    string PhoneNumber,
    string Purpose
) : IRequest<Result>;
