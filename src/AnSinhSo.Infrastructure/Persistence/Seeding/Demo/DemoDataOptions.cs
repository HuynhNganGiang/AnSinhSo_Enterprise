namespace AnSinhSo.Infrastructure.Persistence.Seeding.Demo;

public class DemoDataOptions
{
    public const string SectionName = "DemoData";

    public bool Enabled { get; set; } = false;

    public int Households { get; set; } = 500;
}