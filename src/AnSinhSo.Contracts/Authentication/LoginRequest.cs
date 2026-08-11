namespace AnSinhSo.Contracts.Authentication;

public sealed record LoginRequest(
    string PhoneNumber,
    string OtpCode,
    string DeviceName
);
