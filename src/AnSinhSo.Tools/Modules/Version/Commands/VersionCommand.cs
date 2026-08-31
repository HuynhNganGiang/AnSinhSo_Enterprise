using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Tools.Core;
using AnSinhSo.Tools.Modules.Version.Services;

namespace AnSinhSo.Tools.Modules.Version.Commands;

public class VersionCommand : IToolCommand
{
    private readonly VersionService _service;

    public VersionCommand(VersionService service)
    {
        _service = service;
    }

    public string Name => "version";
    public string Description => "Executes the Version module.";

    public async Task ExecuteAsync(string[] args, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("VersionCommand executing...");
        await _service.ExecuteAsync(cancellationToken);
    }
}
