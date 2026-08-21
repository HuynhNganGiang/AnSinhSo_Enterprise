using System;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate.ValueObjects;

namespace AnSinhSo.Application.Abstractions.Authentication;

public interface IJwtProvider
{
    string GenerateAccessToken(CitizenIdentity identity, UserSessionId userSessionId);
    string GenerateAccessTokenForUser(AnSinhSo.Domain.Aggregates.UserAggregate.User user, AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects.DeviceSessionId deviceSessionId);
}
