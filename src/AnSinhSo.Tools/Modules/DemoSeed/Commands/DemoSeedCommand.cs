using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Tools.Core;
using AnSinhSo.Tools.Modules.DemoSeed.Services;

namespace AnSinhSo.Tools.Modules.DemoSeed.Commands;

public class DemoSeedCommand : IToolCommand
{
    private readonly DemoSeedService _service;

    public DemoSeedCommand(DemoSeedService service)
    {
        _service = service;
    }

    public string Name => "demoseed";
    public string Description => "Executes the DemoSeed module.";

    public async Task ExecuteAsync(string[] args, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("DemoSeedCommand executing...");
        await _service.ExecuteAsync(cancellationToken);
    }
}
