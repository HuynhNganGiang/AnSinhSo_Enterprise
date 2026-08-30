using AnSinhSo.Worker;
using AnSinhSo.Application;
using AnSinhSo.Infrastructure;

const string DataImportMode = "data-import";

var mode = args.FirstOrDefault()?.Trim().ToLowerInvariant();

if (mode is not DataImportMode)
{
    Console.Error.WriteLine(
        "Usage: AnSinhSo.Worker data-import");

    Environment.ExitCode = 2;
    return;
}

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<DataImportWorker>();

var host = builder.Build();
host.Run();
