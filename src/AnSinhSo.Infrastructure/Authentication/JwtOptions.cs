namespace AnSinhSo.Infrastructure.Authentication;

public class JwtOptions
{
    public const string SectionName = "Authentication";

    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}
