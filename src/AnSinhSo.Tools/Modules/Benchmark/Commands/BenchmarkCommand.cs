using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Tools.Core;
using AnSinhSo.Tools.Modules.Benchmark.Services;

namespace AnSinhSo.Tools.Modules.Benchmark.Commands;

public class BenchmarkCommand : IToolCommand
{
    private readonly BenchmarkService _service;

    public BenchmarkCommand(BenchmarkService service)
    {
        _service = service;
    }

    public string Name => "benchmark";
    public string Description => "Executes the Benchmark module.";

    public async Task ExecuteAsync(string[] args, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("BenchmarkCommand executing...");
        await _service.ExecuteAsync(cancellationToken);
    }
}
