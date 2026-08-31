using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using AnSinhSo.Tools.Core;
using AnSinhSo.Tools.Modules.Seed.Commands;
using AnSinhSo.Tools.Modules.Seed.Services;
using AnSinhSo.Tools.Modules.DemoSeed.Commands;
using AnSinhSo.Tools.Modules.DemoSeed.Services;
using AnSinhSo.Tools.Modules.Validation.Commands;
using AnSinhSo.Tools.Modules.Validation.Services;
using AnSinhSo.Tools.Modules.Statistics.Commands;
using AnSinhSo.Tools.Modules.Statistics.Services;
using AnSinhSo.Tools.Modules.Repair.Commands;
using AnSinhSo.Tools.Modules.Repair.Services;
using AnSinhSo.Tools.Modules.Export.Commands;
using AnSinhSo.Tools.Modules.Export.Services;
using AnSinhSo.Tools.Modules.Benchmark.Commands;
using AnSinhSo.Tools.Modules.Benchmark.Services;
using AnSinhSo.Tools.Modules.Diagnose.Commands;
using AnSinhSo.Tools.Modules.Diagnose.Services;
using AnSinhSo.Tools.Modules.Health.Commands;
using AnSinhSo.Tools.Modules.Health.Services;
using AnSinhSo.Tools.Modules.Version.Commands;
using AnSinhSo.Tools.Modules.Version.Services;

namespace AnSinhSo.Tools;

class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        // Register Services
        services.AddTransient<SeedService>();
        services.AddTransient<DemoSeedService>();
        services.AddTransient<ValidationService>();
        services.AddTransient<StatisticsService>();
        services.AddTransient<RepairService>();
        services.AddTransient<ExportService>();
        services.AddTransient<BenchmarkService>();
        services.AddTransient<DiagnoseService>();
        services.AddTransient<HealthService>();
        services.AddTransient<VersionService>();

        // Register Registry and Dispatcher
        var registry = new ToolRegistry(services);
        services.AddSingleton(registry);
        services.AddTransient<CommandDispatcher>();

        // Register Commands
        registry
            .RegisterCommand<SeedCommand>("seed")
            .RegisterCommand<DemoSeedCommand>("demoseed")
            .RegisterCommand<ValidationCommand>("validate")
            .RegisterCommand<StatisticsCommand>("statistics")
            .RegisterCommand<RepairCommand>("repair")
            .RegisterCommand<ExportCommand>("export")
            .RegisterCommand<BenchmarkCommand>("benchmark")
            .RegisterCommand<DiagnoseCommand>("diagnose")
            .RegisterCommand<HealthCommand>("health")
            .RegisterCommand<VersionCommand>("version");

        var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<CommandDispatcher>();

        await dispatcher.DispatchAsync(args);
    }
}
