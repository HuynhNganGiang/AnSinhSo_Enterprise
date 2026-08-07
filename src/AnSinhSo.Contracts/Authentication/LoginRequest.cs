namespace AnSinhSo.Contracts.Authentication;

public record LoginRequest(string UsernameOrEmail, string Password);
