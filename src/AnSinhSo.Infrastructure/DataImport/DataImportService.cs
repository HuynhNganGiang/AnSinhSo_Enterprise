using System.IO;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CsvHelper;
using CsvHelper.Configuration;
using AnSinhSo.Application.DataImport;
using AnSinhSo.Infrastructure.DataImport.Models;

namespace AnSinhSo.Infrastructure.DataImport;

public class DataImportService : IDataImportService
{
    private readonly ILogger<DataImportService> _logger;

    public DataImportService(ILogger<DataImportService> logger)
    {
        _logger = logger;
    }

    public async Task ExecuteImportAsync(CancellationToken cancellationToken = default)
    {
        string dataFolder = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../../../Data");
        
        // Ensure path resolves properly from worker execution path
        if (!Directory.Exists(dataFolder))
        {
            dataFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            if (!Directory.Exists(dataFolder))
            {
                dataFolder = Path.Combine(Directory.GetCurrentDirectory(), "../../../../Data");
                dataFolder = Path.GetFullPath(dataFolder);
            }
        }

        await ProcessCsvFileAsync<Stg_RoleRecord>(Path.Combine(dataFolder, "Stg_Roles.csv"), cancellationToken);
        await ProcessCsvFileAsync<Stg_UserRecord>(Path.Combine(dataFolder, "Stg_Users.csv"), cancellationToken);
        await ProcessCsvFileAsync<Stg_DiaBanRecord>(Path.Combine(dataFolder, "Stg_DiaBan.csv"), cancellationToken);
        await ProcessCsvFileAsync<Stg_NhomDoiTuongRecord>(Path.Combine(dataFolder, "Stg_NhomDoiTuong.csv"), cancellationToken);
        await ProcessCsvFileAsync<Stg_ChinhSachTroCapRecord>(Path.Combine(dataFolder, "Stg_ChinhSachTroCap.csv"), cancellationToken);
        await ProcessCsvFileAsync<Stg_DotChiTraRecord>(Path.Combine(dataFolder, "Stg_DotChiTra.csv"), cancellationToken);
        await ProcessCsvFileAsync<Stg_HoGiaDinhRecord>(Path.Combine(dataFolder, "Stg_HoGiaDinh.csv"), cancellationToken);
        await ProcessCsvFileAsync<Stg_ThanhVienHoGiaDinhRecord>(Path.Combine(dataFolder, "Stg_ThanhVienHoGiaDinh.csv"), cancellationToken);
        await ProcessCsvFileAsync<Stg_LichSuHoGiaDinhRecord>(Path.Combine(dataFolder, "Stg_LichSuHoGiaDinh.csv"), cancellationToken);
        await ProcessCsvFileAsync<Stg_DoiTuongAnSinhRecord>(Path.Combine(dataFolder, "Stg_DoiTuongAnSinh.csv"), cancellationToken);
        await ProcessCsvFileAsync<Stg_ChiTraTroCapRecord>(Path.Combine(dataFolder, "Stg_ChiTraTroCap.csv"), cancellationToken);

        _logger.LogInformation("Import completed.");
    }

    private async Task ProcessCsvFileAsync<T>(string filePath, CancellationToken cancellationToken)
    {
        var fileName = Path.GetFileName(filePath);
        _logger.LogInformation($"Reading {fileName}");
        
        if (!File.Exists(filePath))
        {
            _logger.LogWarning($"File not found: {filePath}");
            return;
        }

        int count = 0;
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            BadDataFound = null,
            HeaderValidated = null
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);

        await foreach (var record in csv.GetRecordsAsync<T>(cancellationToken))
        {
            count++;
        }

        _logger.LogInformation($"{count} records");
    }
}
