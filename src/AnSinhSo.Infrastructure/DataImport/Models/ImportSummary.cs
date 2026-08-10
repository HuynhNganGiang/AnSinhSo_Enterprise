using System;

namespace AnSinhSo.Infrastructure.DataImport.Models;

public class ImportSummary
{
    public string FileName { get; set; } = string.Empty;
    public int TotalRecords { get; set; }
    public int Imported { get; set; }
    public int Skipped { get; set; }
    public int Deferred { get; set; }
    public int ValidationFailed { get; set; }
    public int MappingFailed { get; set; }
    public int Failed { get; set; }
    public TimeSpan Duration { get; set; }
    public double RecordsPerSecond => Duration.TotalSeconds > 0 ? TotalRecords / Duration.TotalSeconds : 0;
    public long MemoryUsageBytes { get; set; }
}
