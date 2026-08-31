using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Tools.Core;
using AnSinhSo.Tools.Modules.Health.Services;

namespace AnSinhSo.Tools.Modules.Health.Commands;

public class HealthCommand : IToolCommand
{
    private readonly HealthService _service;

    public HealthCommand(HealthService service)
    {
        _service = service;
    }

    public string Name => "health";
    public string Description => "Executes the Health module.";

    public async Task ExecuteAsync(string[] args, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("HealthCommand executing...");
        await _service.ExecuteAsync(cancellationToken);
    }
}
