using System;

namespace AnSinhSo.Contracts.CitizenIdentities;

public record RegisterCitizenIdentityRequest(string CitizenId, string PhoneNumber, string FullName);
