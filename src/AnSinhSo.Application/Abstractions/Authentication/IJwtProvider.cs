using System;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;


namespace AnSinhSo.Application.Abstractions.Authentication;

public interface IJwtProvider
{
    string GenerateAccessToken(CitizenIdentity identity, AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects.DeviceSessionId deviceSessionId);
    string GenerateAccessTokenForUser(AnSinhSo.Domain.Aggregates.UserAggregate.User user, AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects.DeviceSessionId deviceSessionId);
}
