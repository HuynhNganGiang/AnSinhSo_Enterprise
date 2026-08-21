using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authentication.Citizen.RequestOtp;

public sealed record RequestCitizenOtpCommand(string PhoneNumber) : IRequest<Result<Guid>>;
