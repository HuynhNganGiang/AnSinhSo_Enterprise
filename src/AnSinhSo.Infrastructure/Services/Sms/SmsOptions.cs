namespace AnSinhSo.Infrastructure.Services.Sms;

public class SmsOptions
{
    public const string SectionName = "SmsProvider";
    public string ApiUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string SenderId { get; set; } = string.Empty;
}
