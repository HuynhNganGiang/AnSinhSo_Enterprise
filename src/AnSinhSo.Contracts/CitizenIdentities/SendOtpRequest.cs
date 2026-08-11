using System;

namespace AnSinhSo.Contracts.CitizenIdentities;

public record SendOtpRequest(string PhoneNumber, string Purpose);
