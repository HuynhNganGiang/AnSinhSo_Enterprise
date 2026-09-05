using System.Data;
using AnSinhSo.Domain.Constants;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.API.Controllers;

[ApiController]
[Route("api/v1/system")]
public sealed class SystemBackupController : ControllerBase
{
    private const string ProductionDatabaseName =
        "AnSinhSoRealDb";

    private static readonly SemaphoreSlim BackupGate =
        new(1, 1);

    private readonly AnSinhSoDbContext _dbContext;
    private readonly ILogger<SystemBackupController> _logger;

    public SystemBackupController(
        AnSinhSoDbContext dbContext,
        ILogger<SystemBackupController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet("backup-status")]
    [Authorize(
        Policy = Permissions.PermissionsModule.View)]
    public async Task<IActionResult> GetStatus(
        CancellationToken cancellationToken)
    {
        var runtimeDatabase =
            GetRuntimeDatabaseName();

        var database =
            await ReadDatabaseAsync(
                cancellationToken);

        var permissions =
            await ReadPermissionsAsync(
                cancellationToken);

        await using var master =
            CreateConnection("master");

        await master.OpenAsync(
            cancellationToken);

        var backupPath =
            await ReadBackupPathAsync(
                master,
                cancellationToken);

        var pathAvailable =
            !string.IsNullOrWhiteSpace(
                backupPath) &&
            Directory.Exists(
                backupPath);

        var productionRuntime =
            string.Equals(
                runtimeDatabase,
                ProductionDatabaseName,
                StringComparison.OrdinalIgnoreCase);

        var createEnabled =
            productionRuntime &&
            database.State == "ONLINE" &&
            !database.IsReadOnly &&
            permissions.CanBackupDatabase &&
            pathAvailable;

        var history =
            await ReadHistoryAsync(
                10,
                null,
                cancellationToken);

        return Ok(
            new
            {
                success = true,

                data = new
                {
                    source =
                        "REAL SQL SERVER",

                    generatedAt =
                        DateTimeOffset.UtcNow,

                    database = new
                    {
                        name =
                            database.Name,

                        runtimeDatabase,

                        isProductionRuntime =
                            productionRuntime,

                        state =
                            database.State,

                        recoveryModel =
                            database.RecoveryModel,

                        userAccess =
                            database.UserAccess,

                        isReadOnly =
                            database.IsReadOnly,

                        approxSizeMB =
                            database.ApproxSizeMB
                    },

                    backup = new
                    {
                        targetDatabase =
                            ProductionDatabaseName,

                        defaultPath =
                            backupPath,

                        pathAvailable,

                        isSysAdmin =
                            permissions.IsSysAdmin,

                        isDbOwner =
                            permissions.IsDbOwner,

                        canBackupDatabase =
                            permissions.CanBackupDatabase,

                        canBackupLog =
                            permissions.CanBackupLog,

                        createEnabled,

                        policy =
                            "FULL COPY_ONLY + CHECKSUM + COMPRESSION"
                    },

                    restore = new
                    {
                        mode =
                            "LOCKED",

                        productionRestoreEnabled =
                            false,

                        reason =
                            "Production restore requires maintenance mode, exclusive access and separate operator confirmation."
                    },

                    history
                }
            });
    }

    [HttpPost("backups")]
    [Authorize(
        Policy = Permissions.PermissionsModule.Manage)]
    public async Task<IActionResult> CreateBackup(
        CancellationToken cancellationToken)
    {
        var entered =
            await BackupGate.WaitAsync(
                0,
                cancellationToken);

        if (!entered)
        {
            return Conflict(
                new
                {
                    success = false,
                    message =
                        "Another backup is already running."
                });
        }

        try
        {
            var runtimeDatabase =
                GetRuntimeDatabaseName();

            if (
                !string.Equals(
                    runtimeDatabase,
                    ProductionDatabaseName,
                    StringComparison.OrdinalIgnoreCase))
            {
                return Conflict(
                    new
                    {
                        success = false,
                        message =
                            "Runtime database is not AnSinhSoRealDb."
                    });
            }

            var database =
                await ReadDatabaseAsync(
                    cancellationToken);

            if (
                database.State != "ONLINE" ||
                database.IsReadOnly)
            {
                return Conflict(
                    new
                    {
                        success = false,
                        message =
                            "Production database is not ready for backup."
                    });
            }

            var permissions =
                await ReadPermissionsAsync(
                    cancellationToken);

            if (!permissions.CanBackupDatabase)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    new
                    {
                        success = false,
                        message =
                            "BACKUP DATABASE permission is unavailable."
                    });
            }

            await using var master =
                CreateConnection("master");

            await master.OpenAsync(
                cancellationToken);

            var backupRoot =
                await ReadBackupPathAsync(
                    master,
                    cancellationToken);

            if (
                string.IsNullOrWhiteSpace(
                    backupRoot) ||
                !Directory.Exists(
                    backupRoot))
            {
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    new
                    {
                        success = false,
                        message =
                            "SQL Server backup path is unavailable."
                    });
            }

            var root =
                Path.GetFullPath(
                    backupRoot);

            var fileName =
                ProductionDatabaseName +
                "_FULL_" +
                DateTime.UtcNow.ToString(
                    "yyyyMMdd_HHmmss_fff") +
                ".bak";

            var physicalDevice =
                Path.GetFullPath(
                    Path.Combine(
                        root,
                        fileName));

            var rootPrefix =
                root.TrimEnd(
                    Path.DirectorySeparatorChar,
                    Path.AltDirectorySeparatorChar) +
                Path.DirectorySeparatorChar;

            if (
                !physicalDevice.StartsWith(
                    rootPrefix,
                    StringComparison.OrdinalIgnoreCase))
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        success = false,
                        message =
                            "Backup path safety check failed."
                    });
            }

            if (System.IO.File.Exists(physicalDevice))
            {
                return Conflict(
                    new
                    {
                        success = false,
                        message =
                            "Backup file already exists."
                    });
            }

            await using (
                var backup =
                    master.CreateCommand())
            {
                backup.CommandTimeout =
                    600;

                backup.CommandText =
$"""
BACKUP DATABASE [{ProductionDatabaseName}]
TO DISK = @backupPath
WITH
    COPY_ONLY,
    CHECKSUM,
    COMPRESSION;
""";

                backup.Parameters.Add(
                    new SqlParameter(
                        "@backupPath",
                        SqlDbType.NVarChar,
                        4000)
                    {
                        Value =
                            physicalDevice
                    });

                await backup.ExecuteNonQueryAsync(
                    cancellationToken);
            }

            await using (
                var verify =
                    master.CreateCommand())
            {
                verify.CommandTimeout =
                    600;

                verify.CommandText =
"""
RESTORE VERIFYONLY
FROM DISK = @backupPath
WITH CHECKSUM;
""";

                verify.Parameters.Add(
                    new SqlParameter(
                        "@backupPath",
                        SqlDbType.NVarChar,
                        4000)
                    {
                        Value =
                            physicalDevice
                    });

                await verify.ExecuteNonQueryAsync(
                    cancellationToken);
            }

            var history =
                await ReadHistoryAsync(
                    1,
                    physicalDevice,
                    cancellationToken);

            var latest =
                history.FirstOrDefault();

            _logger.LogInformation(
                "Verified COPY_ONLY backup created for {DatabaseName}. File={BackupFile}",
                ProductionDatabaseName,
                fileName);

            return Ok(
                new
                {
                    success = true,

                    data = new
                    {
                        source =
                            "REAL SQL SERVER",

                        database =
                            ProductionDatabaseName,

                        fileName,

                        verified =
                            true,

                        backupType =
                            latest?.BackupType ??
                            "FULL",

                        backupFinish =
                            latest?.BackupFinish,

                        backupSizeMB =
                            latest?.BackupSizeMB,

                        copyOnly =
                            latest?.CopyOnly ??
                            true
                    }
                });
        }
        catch (SqlException exception)
        {
            _logger.LogError(
                exception,
                "SQL Server backup failed for {DatabaseName}.",
                ProductionDatabaseName);

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    success = false,
                    message =
                        "SQL Server backup failed.",
                    sqlErrorNumber =
                        exception.Number
                });
        }
        finally
        {
            BackupGate.Release();
        }
    }

    private string GetRuntimeDatabaseName()
    {
        var connectionString =
            _dbContext
                .Database
                .GetDbConnection()
                .ConnectionString;

        if (
            string.IsNullOrWhiteSpace(
                connectionString))
        {
            return string.Empty;
        }

        var builder =
            new SqlConnectionStringBuilder(
                connectionString);

        return builder.InitialCatalog;
    }

    private SqlConnection CreateConnection(
        string databaseName)
    {
        var connectionString =
            _dbContext
                .Database
                .GetDbConnection()
                .ConnectionString;

        if (
            string.IsNullOrWhiteSpace(
                connectionString))
        {
            throw new InvalidOperationException(
                "Runtime SQL connection is unavailable.");
        }

        var builder =
            new SqlConnectionStringBuilder(
                connectionString);

        builder.InitialCatalog =
            databaseName;

        return new SqlConnection(
            builder.ConnectionString);
    }

    private async Task<DatabaseInfo>
        ReadDatabaseAsync(
            CancellationToken cancellationToken)
    {
        await using var connection =
            CreateConnection("master");

        await connection.OpenAsync(
            cancellationToken);

        await using var command =
            connection.CreateCommand();

        command.CommandText =
"""
SELECT
    d.name,
    d.state_desc,
    d.recovery_model_desc,
    d.user_access_desc,
    d.is_read_only,
    CAST(
        SUM(CONVERT(bigint, mf.size))
        * 8.0 / 1024.0
        AS decimal(18, 2)
    )
FROM sys.databases d
INNER JOIN sys.master_files mf
    ON mf.database_id = d.database_id
WHERE d.name = @databaseName
GROUP BY
    d.name,
    d.state_desc,
    d.recovery_model_desc,
    d.user_access_desc,
    d.is_read_only;
""";

        command.Parameters.Add(
            new SqlParameter(
                "@databaseName",
                SqlDbType.NVarChar,
                128)
            {
                Value =
                    ProductionDatabaseName
            });

        await using var reader =
            await command.ExecuteReaderAsync(
                cancellationToken);

        if (
            !await reader.ReadAsync(
                cancellationToken))
        {
            throw new InvalidOperationException(
                "AnSinhSoRealDb was not found.");
        }

        return new DatabaseInfo(
            reader.GetString(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetBoolean(4),
            reader.GetDecimal(5));
    }

    private async Task<BackupPermissions>
        ReadPermissionsAsync(
            CancellationToken cancellationToken)
    {
        await using var connection =
            CreateConnection(
                ProductionDatabaseName);

        await connection.OpenAsync(
            cancellationToken);

        await using var command =
            connection.CreateCommand();

        command.CommandText =
"""
SELECT
    IS_SRVROLEMEMBER('sysadmin'),
    IS_MEMBER('db_owner'),
    HAS_PERMS_BY_NAME(
        DB_NAME(),
        'DATABASE',
        'BACKUP DATABASE'
    ),
    HAS_PERMS_BY_NAME(
        DB_NAME(),
        'DATABASE',
        'BACKUP LOG'
    );
""";

        await using var reader =
            await command.ExecuteReaderAsync(
                cancellationToken);

        if (
            !await reader.ReadAsync(
                cancellationToken))
        {
            throw new InvalidOperationException(
                "Unable to read backup permissions.");
        }

        return new BackupPermissions(
            reader.GetInt32(0) == 1,
            reader.GetInt32(1) == 1,
            reader.GetInt32(2) == 1,
            reader.GetInt32(3) == 1);
    }

    private static async Task<string?>
        ReadBackupPathAsync(
            SqlConnection connection,
            CancellationToken cancellationToken)
    {
        await using var command =
            connection.CreateCommand();

        command.CommandText =
"""
SELECT CAST(
    SERVERPROPERTY(
        'InstanceDefaultBackupPath'
    )
    AS nvarchar(4000)
);
""";

        var value =
            await command.ExecuteScalarAsync(
                cancellationToken);

        if (
            value is null ||
            value == DBNull.Value)
        {
            return null;
        }

        return Convert.ToString(value);
    }

    private async Task<IReadOnlyList<BackupHistory>>
        ReadHistoryAsync(
            int take,
            string? physicalDevice,
            CancellationToken cancellationToken)
    {
        await using var connection =
            CreateConnection("msdb");

        await connection.OpenAsync(
            cancellationToken);

        await using var command =
            connection.CreateCommand();

        command.CommandText =
"""
SELECT TOP (@take)
    bs.backup_set_id,
    CASE bs.type
        WHEN 'D' THEN 'FULL'
        WHEN 'I' THEN 'DIFFERENTIAL'
        WHEN 'L' THEN 'LOG'
        ELSE bs.type
    END,
    bs.backup_start_date,
    bs.backup_finish_date,
    CAST(
        COALESCE(
            NULLIF(
                bs.compressed_backup_size,
                0
            ),
            bs.backup_size
        ) / 1024.0 / 1024.0
        AS decimal(18, 2)
    ),
    bmf.physical_device_name,
    bs.is_copy_only
FROM dbo.backupset bs
LEFT JOIN dbo.backupmediafamily bmf
    ON bmf.media_set_id =
       bs.media_set_id
WHERE
    bs.database_name =
        @databaseName
    AND
    (
        @physicalDevice IS NULL
        OR
        bmf.physical_device_name =
            @physicalDevice
    )
ORDER BY
    bs.backup_finish_date DESC;
""";

        command.Parameters.Add(
            new SqlParameter(
                "@take",
                SqlDbType.Int)
            {
                Value =
                    Math.Clamp(
                        take,
                        1,
                        50)
            });

        command.Parameters.Add(
            new SqlParameter(
                "@databaseName",
                SqlDbType.NVarChar,
                128)
            {
                Value =
                    ProductionDatabaseName
            });

        command.Parameters.Add(
            new SqlParameter(
                "@physicalDevice",
                SqlDbType.NVarChar,
                4000)
            {
                Value =
                    physicalDevice is null
                        ? DBNull.Value
                        : physicalDevice
            });

        var result =
            new List<BackupHistory>();

        await using var reader =
            await command.ExecuteReaderAsync(
                cancellationToken);

        while (
            await reader.ReadAsync(
                cancellationToken))
        {
            result.Add(
                new BackupHistory(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetDateTime(2),
                    reader.GetDateTime(3),
                    reader.GetDecimal(4),
                    reader.IsDBNull(5)
                        ? null
                        : reader.GetString(5),
                    reader.GetBoolean(6)));
        }

        return result;
    }

    private sealed record DatabaseInfo(
        string Name,
        string State,
        string RecoveryModel,
        string UserAccess,
        bool IsReadOnly,
        decimal ApproxSizeMB);

    private sealed record BackupPermissions(
        bool IsSysAdmin,
        bool IsDbOwner,
        bool CanBackupDatabase,
        bool CanBackupLog);

    private sealed record BackupHistory(
        int BackupSetId,
        string BackupType,
        DateTime BackupStart,
        DateTime BackupFinish,
        decimal BackupSizeMB,
        string? PhysicalDevice,
        bool CopyOnly);
}