using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Tools.Core;
using AnSinhSo.Tools.Modules.Export.Services;

namespace AnSinhSo.Tools.Modules.Export.Commands;

public class ExportCommand : IToolCommand
{
    private readonly ExportService _service;

    public ExportCommand(ExportService service)
    {
        _service = service;
    }

    public string Name => "export";
    public string Description => "Executes the Export module.";

    public async Task ExecuteAsync(string[] args, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("ExportCommand executing...");
        await _service.ExecuteAsync(cancellationToken);
    }
}
