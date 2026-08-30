namespace AnSinhSo.Infrastructure.DataImport.Options;

public class ImportOptions
{
    public const string SectionName = "ImportOptions";

    public int BatchSize { get; set; } = 1000;

    public string DataPath { get; set; } = string.Empty;
}
