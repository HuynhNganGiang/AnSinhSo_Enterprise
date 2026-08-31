using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Tools.Modules.Diagnose.Services;

public class DiagnoseService
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("DiagnoseService logic pending...");
        return Task.CompletedTask;
    }
}
