using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Tools.Modules.Version.Services;

public class VersionService
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("VersionService logic pending...");
        return Task.CompletedTask;
    }
}
