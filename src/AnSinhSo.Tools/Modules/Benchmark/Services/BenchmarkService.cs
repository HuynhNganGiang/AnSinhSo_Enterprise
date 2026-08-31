using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Tools.Modules.Benchmark.Services;

public class BenchmarkService
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("BenchmarkService logic pending...");
        return Task.CompletedTask;
    }
}
