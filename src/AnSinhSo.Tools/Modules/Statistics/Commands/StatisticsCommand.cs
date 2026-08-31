using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Tools.Core;
using AnSinhSo.Tools.Modules.Statistics.Services;

namespace AnSinhSo.Tools.Modules.Statistics.Commands;

public class StatisticsCommand : IToolCommand
{
    private readonly StatisticsService _service;

    public StatisticsCommand(StatisticsService service)
    {
        _service = service;
    }

    public string Name => "statistics";
    public string Description => "Executes the Statistics module.";

    public async Task ExecuteAsync(string[] args, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("StatisticsCommand executing...");
        await _service.ExecuteAsync(cancellationToken);
    }
}
