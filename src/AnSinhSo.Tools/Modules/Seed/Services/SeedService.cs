using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Tools.Modules.Seed.Services;

public class SeedService
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("SeedService logic pending...");
        return Task.CompletedTask;
    }
}
