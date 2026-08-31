using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Tools.Core;
using AnSinhSo.Tools.Modules.Validation.Services;

namespace AnSinhSo.Tools.Modules.Validation.Commands;

public class ValidationCommand : IToolCommand
{
    private readonly ValidationService _service;

    public ValidationCommand(ValidationService service)
    {
        _service = service;
    }

    public string Name => "validation";
    public string Description => "Executes the Validation module.";

    public async Task ExecuteAsync(string[] args, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("ValidationCommand executing...");
        await _service.ExecuteAsync(cancellationToken);
    }
}
