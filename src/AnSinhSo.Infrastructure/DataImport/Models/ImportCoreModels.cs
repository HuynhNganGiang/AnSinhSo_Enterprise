using System;

namespace AnSinhSo.Infrastructure.DataImport.Models;

public enum ImportStatus
{
    Imported,
    Skipped,
    Deferred,
    Failed,
    Validation,
    Mapping
}

public enum ImportErrorCode
{
    NONE,
    INVALID_CCCD,
    INVALID_DATE,
    DUPLICATE_KEY,
    DOMAIN_RULE,
    CSV_FORMAT,
    DATA_TYPE_MISMATCH,
    MISSING_REQUIRED_FIELD,
    UNKNOWN_ERROR
}

public class ImportContext
{
    public Guid ImportId { get; }
    public Guid CorrelationId { get; }
    public string FileName { get; }
    public int LineNumber { get; }
    public DateTime StartedAt { get; }

    public ImportContext(Guid importId, Guid correlationId, string fileName, int lineNumber, DateTime startedAt)
    {
        ImportId = importId;
        CorrelationId = correlationId;
        FileName = fileName;
        LineNumber = lineNumber;
        StartedAt = startedAt;
    }
}

public class ImportResult<T>
{
    public ImportStatus Status { get; }
    public ImportErrorCode ErrorCode { get; }
    public string? ErrorMessage { get; }
    public T? Entity { get; }

    private ImportResult(ImportStatus status, ImportErrorCode errorCode, string? errorMessage, T? entity)
    {
        Status = status;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        Entity = entity;
    }

    public static ImportResult<T> Success(T entity) => 
        new(ImportStatus.Imported, ImportErrorCode.NONE, null, entity);

    public static ImportResult<T> Failure(ImportErrorCode errorCode, string errorMessage) => 
        new(ImportStatus.Failed, errorCode, errorMessage, default);

    public static ImportResult<T> Deferred(string reason) => 
        new(ImportStatus.Deferred, ImportErrorCode.NONE, reason, default);

    public static ImportResult<T> Skipped(string reason) => 
        new(ImportStatus.Skipped, ImportErrorCode.NONE, reason, default);
}
