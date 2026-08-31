using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Tools.Modules.Health.Services;

public class HealthService
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("HealthService logic pending...");
        return Task.CompletedTask;
    }
}
