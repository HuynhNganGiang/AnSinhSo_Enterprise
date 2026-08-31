using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Tools.Core;
using AnSinhSo.Tools.Modules.Repair.Services;

namespace AnSinhSo.Tools.Modules.Repair.Commands;

public class RepairCommand : IToolCommand
{
    private readonly RepairService _service;

    public RepairCommand(RepairService service)
    {
        _service = service;
    }

    public string Name => "repair";
    public string Description => "Executes the Repair module.";

    public async Task ExecuteAsync(string[] args, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("RepairCommand executing...");
        await _service.ExecuteAsync(cancellationToken);
    }
}
