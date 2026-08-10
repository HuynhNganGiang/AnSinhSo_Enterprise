using System;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CsvHelper;
using CsvHelper.Configuration;
using AnSinhSo.Application.DataImport;
using AnSinhSo.Infrastructure.DataImport.Models;
using AnSinhSo.Infrastructure.DataImport.Mappers;
using AnSinhSo.Infrastructure.DataImport.Normalization;
using AnSinhSo.Domain.Aggregates.UserAggregate;
using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;

namespace AnSinhSo.Infrastructure.DataImport;

public class DataImportService : IDataImportService
{
    private readonly ILogger<DataImportService> _logger;
    private readonly ICsvStringNormalizer _normalizer;
    private readonly IWelfareGroupMapper _welfareGroupMapper;
    private readonly ICitizenMapper _citizenMapper;
    private readonly IHouseholdMapper _householdMapper;
    private readonly IPolicyMapper _policyMapper;
    private readonly IPaymentMapper _paymentMapper;

    public DataImportService(
        ILogger<DataImportService> logger,
        ICsvStringNormalizer normalizer,
        IWelfareGroupMapper welfareGroupMapper,
        ICitizenMapper citizenMapper,
        IHouseholdMapper householdMapper,
        IPolicyMapper policyMapper,
        IPaymentMapper paymentMapper)
    {
        _logger = logger;
        _normalizer = normalizer;
        _welfareGroupMapper = welfareGroupMapper;
        _citizenMapper = citizenMapper;
        _householdMapper = householdMapper;
        _policyMapper = policyMapper;
        _paymentMapper = paymentMapper;
    }

    public async Task ExecuteImportAsync(CancellationToken cancellationToken = default)
    {
        var importId = Guid.NewGuid();
        var correlationId = Guid.NewGuid(); // To be replaced by proper distributed tracing ID in future
        var startedAt = DateTime.UtcNow;

        string dataFolder = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../../../Data");
        if (!Directory.Exists(dataFolder))
        {
            dataFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            if (!Directory.Exists(dataFolder))
            {
                dataFolder = Path.Combine(Directory.GetCurrentDirectory(), "../../../../Data");
                dataFolder = Path.GetFullPath(dataFolder);
            }
        }

        // 1 & 2. Roles & Users are ALREADY IMPORTED per Architecture Decision #2
        _logger.LogInformation("Skipping Stg_Roles.csv and Stg_Users.csv (Already Imported).");
        
        // 3. Stg_DiaBan.csv (Deferred per plan)
        await ProcessCsvFileAsync<Stg_DiaBanRecord>(importId, correlationId, startedAt, Path.Combine(dataFolder, "Stg_DiaBan.csv"), cancellationToken);
        
        // 4. Stg_NhomDoiTuong.csv
        await ProcessCsvFileAsync<Stg_NhomDoiTuongRecord, WelfareGroup>(
            importId, correlationId, startedAt,
            Path.Combine(dataFolder, "Stg_NhomDoiTuong.csv"), 
            _welfareGroupMapper, 
            cancellationToken);
        
        // 5, 6, 7 (Skipped mapping temporarily until full mappers implemented, falling back to parse-only)
        await ProcessCsvFileAsync<Stg_ChinhSachTroCapRecord, Policy>(importId, correlationId, startedAt, Path.Combine(dataFolder, "Stg_ChinhSachTroCap.csv"), _policyMapper, cancellationToken);
        await ProcessCsvFileAsync<Stg_DotChiTraRecord, Payment>(importId, correlationId, startedAt, Path.Combine(dataFolder, "Stg_DotChiTra.csv"), _paymentMapper, cancellationToken);
        await ProcessCsvFileAsync<Stg_HoGiaDinhRecord, Household>(importId, correlationId, startedAt, Path.Combine(dataFolder, "Stg_HoGiaDinh.csv"), _householdMapper, cancellationToken);
        
        // 8, 9 (No aggregate mapping logic yet)
        await ProcessCsvFileAsync<Stg_ThanhVienHoGiaDinhRecord>(importId, correlationId, startedAt, Path.Combine(dataFolder, "Stg_ThanhVienHoGiaDinh.csv"), cancellationToken);
        
        // Lich Su Ho Gia Dinh is deferred because there is no aggregate
        await ProcessCsvFileAsync<Stg_LichSuHoGiaDinhRecord>(importId, correlationId, startedAt, Path.Combine(dataFolder, "Stg_LichSuHoGiaDinh.csv"), cancellationToken);
        
        // 10. Stg_DoiTuongAnSinh.csv
        await ProcessCsvFileAsync<Stg_DoiTuongAnSinhRecord, Citizen>(
            importId, correlationId, startedAt,
            Path.Combine(dataFolder, "Stg_DoiTuongAnSinh.csv"), 
            _citizenMapper, 
            cancellationToken);
        
        // 11. Stg_ChiTraTroCap.csv
        await ProcessCsvFileAsync<Stg_ChiTraTroCapRecord>(importId, correlationId, startedAt, Path.Combine(dataFolder, "Stg_ChiTraTroCap.csv"), cancellationToken);

        _logger.LogInformation("Import completed.");
    }

    private async Task ProcessCsvFileAsync<T>(Guid importId, Guid correlationId, DateTime startedAt, string filePath, CancellationToken cancellationToken)
    {
        var fileName = Path.GetFileName(filePath);
        _logger.LogInformation($"Reading {fileName} [Parse-Only/Deferred]");
        
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

        _logger.LogInformation($"[Parse-Only] {count} records read.");
    }

    private async Task ProcessCsvFileAsync<TDto, TEntity>(
        Guid importId, 
        Guid correlationId, 
        DateTime startedAt, 
        string filePath, 
        IDataMapper<TDto, TEntity> mapper, 
        CancellationToken cancellationToken)
    {
        var fileName = Path.GetFileName(filePath);
        _logger.LogInformation($"Reading and Mapping {fileName}");
        
        if (!File.Exists(filePath))
        {
            _logger.LogWarning($"File not found: {filePath}");
            return;
        }

        int count = 0;
        int successCount = 0;
        int failCount = 0;
        int deferredCount = 0;

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            BadDataFound = null,
            HeaderValidated = null
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);

        await foreach (var record in csv.GetRecordsAsync<TDto>(cancellationToken))
        {
            count++;
            var context = new ImportContext(importId, correlationId, fileName, count, startedAt);
            
            // Normalize DTO fields here before mapping using reflection or manual mapping
            NormalizeDtoProperties(record);

            var mapResult = mapper.Map(context, record);

            switch (mapResult.Status)
            {
                case ImportStatus.Imported:
                    successCount++;
                    break;
                case ImportStatus.Failed:
                    failCount++;
                    _logger.LogWarning($"[Failed] Line {count}: {mapResult.ErrorCode} - {mapResult.ErrorMessage}");
                    break;
                case ImportStatus.Deferred:
                    deferredCount++;
                    break;
            }
        }

        _logger.LogInformation($"Mapping completed. Total: {count}, Success: {successCount}, Failed: {failCount}, Deferred: {deferredCount}.");
    }

    private void NormalizeDtoProperties<TDto>(TDto dto)
    {
        if (dto == null) return;
        var properties = typeof(TDto).GetProperties();
        foreach (var prop in properties)
        {
            if (prop.PropertyType == typeof(string) && prop.CanWrite)
            {
                var val = prop.GetValue(dto) as string;
                if (val != null)
                {
                    prop.SetValue(dto, _normalizer.Normalize(val));
                }
            }
        }
    }
}
