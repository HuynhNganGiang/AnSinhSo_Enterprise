using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Tools.Modules.DemoSeed.Services;

public class DemoSeedService
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("DemoSeedService logic pending...");
        return Task.CompletedTask;
    }
}
