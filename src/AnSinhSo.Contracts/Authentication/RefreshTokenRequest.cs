namespace AnSinhSo.Contracts.Authentication;

public sealed record RefreshTokenRequest(
    string RefreshToken,
    string DeviceName
);
