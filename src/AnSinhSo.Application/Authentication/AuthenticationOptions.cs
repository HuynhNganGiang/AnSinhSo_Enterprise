namespace AnSinhSo.Application.Authentication;

public class AuthenticationOptions
{
    public const string SectionName = "Authentication";

    public int AccessTokenLifetimeMinutes { get; set; } = 15;
    public int RefreshTokenLifetimeDays { get; set; } = 30;
}
