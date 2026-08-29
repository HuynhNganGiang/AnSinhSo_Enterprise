using System;

namespace AnSinhSo.Application.Authentication.BackOffice.RefreshToken.Resolvers;

public sealed record RefreshSessionContext(
    Guid? CitizenIdentityId,
    Guid SessionId,
    Guid FamilyId,
    Guid? OldRefreshTokenId = null,
    Guid? UserId = null
);
