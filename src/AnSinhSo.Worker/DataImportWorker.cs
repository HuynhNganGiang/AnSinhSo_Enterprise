namespace AnSinhSo.Worker;

public sealed class DataImportWorker : BackgroundService
{
    private readonly ILogger<DataImportWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public DataImportWorker(ILogger<DataImportWorker> logger, IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Worker starting at: {time}", DateTimeOffset.Now);

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dataImportService = scope.ServiceProvider.GetRequiredService<AnSinhSo.Application.DataImport.IDataImportService>();
                
                await dataImportService.ExecuteImportAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during Data Import Pipeline execution.");
            }

            _logger.LogInformation("Worker finished at: {time}", DateTimeOffset.Now);
        }
        finally
        {
            _hostApplicationLifetime.StopApplication();
        }
    }
}
