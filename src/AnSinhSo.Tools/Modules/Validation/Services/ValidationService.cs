using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Tools.Modules.Validation.Services;

public class ValidationService
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("ValidationService logic pending...");
        return Task.CompletedTask;
    }
}
