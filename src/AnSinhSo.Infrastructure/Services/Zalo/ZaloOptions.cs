namespace AnSinhSo.Infrastructure.Services.Zalo;

public class ZaloOptions
{
    public const string SectionName = "ZaloOA";
    public string AppId { get; set; } = string.Empty;
    public string AppSecret { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
