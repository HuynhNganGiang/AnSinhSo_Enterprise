namespace AnSinhSo.Application.Authentication.Citizen;

public class OtpOptions
{
    public const string SectionName = "Otp";

    public int MaxFailedAttempts { get; set; } = 5;
    public int ExpiryMinutes { get; set; } = 5;
    public int CooldownSeconds { get; set; } = 60;
    public int CodeLength { get; set; } = 6;
    public int MaxRequestsPerHour { get; set; } = 5;
    public int MaxRequestsPerDay { get; set; } = 20;
    public int MaxDeviceRequestsPerHour { get; set; } = 15;
    public int MaxDeviceRequestsPerDay { get; set; } = 50;
}
