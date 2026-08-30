using AnSinhSo.Worker;
using AnSinhSo.Application;
using AnSinhSo.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<DataImportWorker>();

var host = builder.Build();
host.Run();
