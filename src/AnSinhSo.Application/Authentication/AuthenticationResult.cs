namespace AnSinhSo.Application.Authentication;

public record AuthenticationResult(
    string AccessToken,
    string RefreshToken,
    int ExpiresInSeconds,
    System.Guid UserId
);
