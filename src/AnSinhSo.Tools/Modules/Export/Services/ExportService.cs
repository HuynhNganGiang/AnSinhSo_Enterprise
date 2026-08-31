using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Tools.Modules.Export.Services;

public class ExportService
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("ExportService logic pending...");
        return Task.CompletedTask;
    }
}
