using System;

namespace AnSinhSo.Contracts.CitizenIdentities;

public record RegisterCitizenIdentityResponse(Guid CitizenIdentityId, string Status);
