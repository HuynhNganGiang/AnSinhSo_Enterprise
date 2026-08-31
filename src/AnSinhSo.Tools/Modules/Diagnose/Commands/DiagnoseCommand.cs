using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Tools.Core;
using AnSinhSo.Tools.Modules.Diagnose.Services;

namespace AnSinhSo.Tools.Modules.Diagnose.Commands;

public class DiagnoseCommand : IToolCommand
{
    private readonly DiagnoseService _service;

    public DiagnoseCommand(DiagnoseService service)
    {
        _service = service;
    }

    public string Name => "diagnose";
    public string Description => "Executes the Diagnose module.";

    public async Task ExecuteAsync(string[] args, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("DiagnoseCommand executing...");
        await _service.ExecuteAsync(cancellationToken);
    }
}
