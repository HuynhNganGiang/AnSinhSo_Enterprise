using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Tools.Core;
using AnSinhSo.Tools.Modules.Seed.Services;

namespace AnSinhSo.Tools.Modules.Seed.Commands;

public class SeedCommand : IToolCommand
{
    private readonly SeedService _service;

    public SeedCommand(SeedService service)
    {
        _service = service;
    }

    public string Name => "seed";
    public string Description => "Executes the Seed module.";

    public async Task ExecuteAsync(string[] args, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("SeedCommand executing...");
        await _service.ExecuteAsync(cancellationToken);
    }
}
