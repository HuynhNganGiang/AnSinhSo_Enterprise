using System;

namespace AnSinhSo.Contracts.Authentication;

public record TokenResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAtUtc,
    int ExpiresInSeconds,
    string TokenType = "Bearer");
