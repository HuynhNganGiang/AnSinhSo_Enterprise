using AnSinhSo.Worker;
using AnSinhSo.Worker.Services;
using AnSinhSo.Application;
using AnSinhSo.Infrastructure;

const string DataImportMode = "data-import";
const string MaintenanceMode = "maintenance";

var mode = args.FirstOrDefault()?.Trim().ToLowerInvariant();

if (mode is not DataImportMode and not MaintenanceMode)
{
    Console.Error.WriteLine(
        "Usage: AnSinhSo.Worker data-import | maintenance");

    Environment.ExitCode = 2;
    return;
}

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

if (mode == DataImportMode)
{
    builder.Services.AddHostedService<DataImportWorker>();
}
else
{
    builder.Services.AddHostedService<SessionCleanupBackgroundService>();
}

var host = builder.Build();
host.Run();
