using System;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate.ValueObjects;

namespace AnSinhSo.Application.Abstractions.Authentication;

public interface IJwtProvider
{
    string GenerateAccessToken(CitizenIdentity identity, UserSessionId userSessionId);
}
