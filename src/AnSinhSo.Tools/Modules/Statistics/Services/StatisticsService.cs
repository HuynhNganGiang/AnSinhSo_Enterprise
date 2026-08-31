using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Tools.Modules.Statistics.Services;

public class StatisticsService
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("StatisticsService logic pending...");
        return Task.CompletedTask;
    }
}
