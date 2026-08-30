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
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Infrastructure.DataImport.Options;
using AnSinhSo.Infrastructure.DataImport.Cache;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace AnSinhSo.Infrastructure.DataImport;

public class DataImportService : IDataImportService
{
    private readonly ILogger<DataImportService> _logger;
    private readonly ICsvStringNormalizer _normalizer;
    
    // Mappers
    private readonly IWelfareGroupMapper _welfareGroupMapper;
    private readonly ICitizenMapper _citizenMapper;
    private readonly IHouseholdMapper _householdMapper;
    private readonly IPolicyMapper _policyMapper;
    private readonly IPaymentMapper _paymentMapper;

    // Repositories & UoW
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IHouseholdRepository _householdRepository;
    private readonly IWelfareGroupRepository _welfareGroupRepository;
    private readonly IPolicyRepository _policyRepository;
    private readonly IPaymentRepository _paymentRepository;

    private readonly ImportOptions _options;
    private readonly ILookupCacheService _lookupCache;

    public DataImportService(
        ILogger<DataImportService> logger,
        ICsvStringNormalizer normalizer,
        IWelfareGroupMapper welfareGroupMapper,
        ICitizenMapper citizenMapper,
        IHouseholdMapper householdMapper,
        IPolicyMapper policyMapper,
        IPaymentMapper paymentMapper,
        IUnitOfWork unitOfWork,
        ICitizenRepository citizenRepository,
        IHouseholdRepository householdRepository,
        IWelfareGroupRepository welfareGroupRepository,
        IPolicyRepository policyRepository,
        IPaymentRepository paymentRepository,
        IOptions<ImportOptions> options,
        ILookupCacheService lookupCache)
    {
        _logger = logger;
        _normalizer = normalizer;
        _welfareGroupMapper = welfareGroupMapper;
        _citizenMapper = citizenMapper;
        _householdMapper = householdMapper;
        _policyMapper = policyMapper;
        _paymentMapper = paymentMapper;
        _unitOfWork = unitOfWork;
        _citizenRepository = citizenRepository;
        _householdRepository = householdRepository;
        _welfareGroupRepository = welfareGroupRepository;
        _policyRepository = policyRepository;
        _paymentRepository = paymentRepository;
        _options = options.Value;
        _lookupCache = lookupCache;
    }

    public async Task ExecuteImportAsync(CancellationToken cancellationToken = default)
    {
        var importId = Guid.NewGuid();
        var correlationId = Guid.NewGuid(); // To be replaced by proper distributed tracing ID in future
        var startedAt = DateTime.UtcNow;

        if (string.IsNullOrWhiteSpace(_options.DataPath))
        {
            _logger.LogError(
                "CSV data path is not configured. Configure {Section}:{Property}.",
                ImportOptions.SectionName,
                nameof(ImportOptions.DataPath));

            return;
        }

        string dataFolder = Path.GetFullPath(_options.DataPath);

        _logger.LogInformation(
            "CSV data folder: {DataFolder}",
            dataFolder);

        if (!Directory.Exists(dataFolder))
        {
            _logger.LogError(
                "CSV data folder not found: {DataFolder}",
                dataFolder);

            return;
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
            _welfareGroupRepository.Add,
            r => r.MaNhom,
            cancellationToken);
        
        // 5, 6, 7 (Skipped mapping temporarily until full mappers implemented, falling back to parse-only)
        await ProcessCsvFileAsync<Stg_ChinhSachTroCapRecord, Policy>(importId, correlationId, startedAt, Path.Combine(dataFolder, "Stg_ChinhSachTroCap.csv"), _policyMapper, _policyRepository.Add, r => r.MaChinhSach, cancellationToken);
        await ProcessCsvFileAsync<Stg_DotChiTraRecord, Payment>(importId, correlationId, startedAt, Path.Combine(dataFolder, "Stg_DotChiTra.csv"), _paymentMapper, _paymentRepository.Add, r => r.MaDotChiTra, cancellationToken);
        await ProcessCsvFileAsync<Stg_HoGiaDinhRecord, Household>(importId, correlationId, startedAt, Path.Combine(dataFolder, "Stg_HoGiaDinh.csv"), _householdMapper, _householdRepository.Add, r => r.MaHo, cancellationToken);
        
        // 8, 9 (No aggregate mapping logic yet)
        await ProcessCsvFileAsync<Stg_ThanhVienHoGiaDinhRecord>(importId, correlationId, startedAt, Path.Combine(dataFolder, "Stg_ThanhVienHoGiaDinh.csv"), cancellationToken);
        
        // Lich Su Ho Gia Dinh is deferred because there is no aggregate
        await ProcessCsvFileAsync<Stg_LichSuHoGiaDinhRecord>(importId, correlationId, startedAt, Path.Combine(dataFolder, "Stg_LichSuHoGiaDinh.csv"), cancellationToken);
        
        // 10. Stg_DoiTuongAnSinh.csv
        await ProcessCsvFileAsync<Stg_DoiTuongAnSinhRecord, Citizen>(
            importId, correlationId, startedAt,
            Path.Combine(dataFolder, "Stg_DoiTuongAnSinh.csv"), 
            _citizenMapper, 
            _citizenRepository.Add,
            r => r.MaDoiTuong,
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
        Action<TEntity>? addToRepository,
        Func<TDto, string?>? cacheKeySelector,
        CancellationToken cancellationToken)
    {
        var fileName = Path.GetFileName(filePath);
        _logger.LogInformation($"Reading and Mapping {fileName}");
        
        var summary = new ImportSummary { FileName = fileName };
        var sw = Stopwatch.StartNew();
        long memBefore = GC.GetTotalMemory(false);

        if (!File.Exists(filePath))
        {
            _logger.LogWarning($"File not found: {filePath}");
            return;
        }

        int currentBatchCount = 0;
        int batchSize = _options.BatchSize;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
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
                summary.TotalRecords++;
                var context = new ImportContext(importId, correlationId, fileName, summary.TotalRecords, startedAt);
                
                NormalizeDtoProperties(record);
                var mapResult = mapper.Map(context, record);

                switch (mapResult.Status)
                {
                    case ImportStatus.Imported:
                        summary.Imported++;
                        if (mapResult.Entity != null)
                        {
                            addToRepository?.Invoke(mapResult.Entity);
                            currentBatchCount++;

                            // Extract Id to cache
                            if (cacheKeySelector != null)
                            {
                                var key = cacheKeySelector(record);
                                if (!string.IsNullOrEmpty(key))
                                {
                                    CacheEntityId(mapResult.Entity, key);
                                }
                            }
                        }
                        break;
                    case ImportStatus.Failed:
                        summary.Failed++;
                        if (mapResult.ErrorCode == ImportErrorCode.DOMAIN_RULE || mapResult.ErrorCode == ImportErrorCode.INVALID_CCCD || mapResult.ErrorCode == ImportErrorCode.INVALID_DATE || mapResult.ErrorCode == ImportErrorCode.DATA_TYPE_MISMATCH || mapResult.ErrorCode == ImportErrorCode.MISSING_REQUIRED_FIELD)
                        {
                            summary.ValidationFailed++;
                        }
                        else
                        {
                            summary.MappingFailed++;
                        }
                        _logger.LogWarning($"[Failed] Line {summary.TotalRecords}: {mapResult.ErrorCode} - {mapResult.ErrorMessage}");
                        break;
                    case ImportStatus.Deferred:
                        summary.Deferred++;
                        break;
                    case ImportStatus.Skipped:
                        summary.Skipped++;
                        break;
                }

                if (currentBatchCount >= batchSize)
                {
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    _unitOfWork.ClearChangeTracker();
                    currentBatchCount = 0;
                }
            }

            if (currentBatchCount > 0)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _unitOfWork.ClearChangeTracker();
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            
            sw.Stop();
            summary.Duration = sw.Elapsed;
            long memAfter = GC.GetTotalMemory(false);
            summary.MemoryUsageBytes = memAfter - memBefore;

            _logger.LogInformation($"Mapping completed for {fileName}. " +
                                   $"Total: {summary.TotalRecords}, " +
                                   $"Imported: {summary.Imported}, " +
                                   $"Failed: {summary.Failed} (Val: {summary.ValidationFailed}, Map: {summary.MappingFailed}), " +
                                   $"Deferred: {summary.Deferred}, " +
                                   $"Skipped: {summary.Skipped}. " +
                                   $"Duration: {summary.Duration.TotalMilliseconds}ms, " +
                                   $"Rate: {summary.RecordsPerSecond:F2} rec/sec, " +
                                   $"Memory Diff: {summary.MemoryUsageBytes / 1024.0 / 1024.0:F2} MB");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogError(ex, $"Error processing {fileName}. Transaction rolled back.");
        }
    }

    private void CacheEntityId<TEntity>(TEntity entity, string key)
    {
        try
        {
            var idProp = typeof(TEntity).GetProperty("Id");
            if (idProp != null)
            {
                var idObj = idProp.GetValue(entity);
                if (idObj != null)
                {
                    var valueProp = idObj.GetType().GetProperty("Value");
                    if (valueProp != null && valueProp.GetValue(idObj) is Guid guidValue)
                    {
                        _lookupCache.Set<TEntity>(key, guidValue);
                    }
                }
            }
        }
        catch { /* Ignore reflection errors */ }
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
