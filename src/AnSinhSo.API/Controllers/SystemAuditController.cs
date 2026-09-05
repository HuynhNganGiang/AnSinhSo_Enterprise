using System.Data;
using System.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnSinhSo.Infrastructure.Persistence.Contexts;

namespace AnSinhSo.API.Controllers;

[ApiController]
[Route("api/v1/system/audit-logs")]
[Authorize]
public sealed class SystemAuditController : ControllerBase
{
    private static readonly string[] Tables =
    [
        "AuditLogins",
        "SecurityLogs",
        "LoginHistories"
    ];

    private readonly AnSinhSoDbContext _dbContext;

    public SystemAuditController(
        AnSinhSoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(
        CancellationToken cancellationToken)
    {
        var connection =
            _dbContext.Database.GetDbConnection();

        var shouldClose =
            connection.State !=
            ConnectionState.Open;

        if (shouldClose)
        {
            await connection.OpenAsync(
                cancellationToken);
        }

        try
        {
            var users =
                await ReadUsersAsync(
                    connection,
                    cancellationToken);

            var counts =
                new Dictionary<string, int>(
                    StringComparer.OrdinalIgnoreCase);

            var items =
                new List<AuditEventDto>();

            foreach (var table in Tables)
            {
                counts[table] =
                    await CountAsync(
                        connection,
                        table,
                        cancellationToken);

                var rows =
                    await ReadRowsAsync(
                        connection,
                        table,
                        500,
                        cancellationToken);

                foreach (var row in rows)
                {
                    items.Add(
                        BuildEvent(
                            table,
                            row,
                            users));
                }
            }

            var ordered =
                items
                    .OrderByDescending(
                        x => x.OccurredAt ??
                             string.Empty,
                        StringComparer.Ordinal)
                    .ToList();

            return Ok(
                new
                {
                    success = true,

                    data = new
                    {
                        source = "REAL",
                        snapshotAt =
                            DateTimeOffset.UtcNow,
                        counts,
                        items = ordered
                    }
                });
        }
        finally
        {
            if (shouldClose)
            {
                await connection.CloseAsync();
            }
        }
    }


    private static async Task<int> CountAsync(
        DbConnection connection,
        string table,
        CancellationToken cancellationToken)
    {
        await using var command =
            connection.CreateCommand();

        command.CommandText =
            $"SELECT COUNT(*) FROM dbo.[{table}];";

        var result =
            await command.ExecuteScalarAsync(
                cancellationToken);

        return Convert.ToInt32(result);
    }


    private static async Task<
        List<Dictionary<string, object?>>>
        ReadRowsAsync(
            DbConnection connection,
            string table,
            int limit,
            CancellationToken cancellationToken)
    {
        await using var command =
            connection.CreateCommand();

        command.CommandText =
            $"SELECT TOP ({limit}) * FROM dbo.[{table}];";

        await using var reader =
            await command.ExecuteReaderAsync(
                cancellationToken);

        var rows =
            new List<
                Dictionary<string, object?>>();

        while (
            await reader.ReadAsync(
                cancellationToken))
        {
            var row =
                new Dictionary<string, object?>(
                    StringComparer.OrdinalIgnoreCase);

            for (
                var i = 0;
                i < reader.FieldCount;
                i++)
            {
                row[reader.GetName(i)] =
                    reader.IsDBNull(i)
                        ? null
                        : reader.GetValue(i);
            }

            rows.Add(row);
        }

        return rows;
    }


    private static async Task<
        Dictionary<string, string>>
        ReadUsersAsync(
            DbConnection connection,
            CancellationToken cancellationToken)
    {
        await using var command =
            connection.CreateCommand();

        command.CommandText =
            """
            SELECT
                CONVERT(varchar(36), Id) AS Id,
                Username
            FROM dbo.Users;
            """;

        await using var reader =
            await command.ExecuteReaderAsync(
                cancellationToken);

        var users =
            new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);

        while (
            await reader.ReadAsync(
                cancellationToken))
        {
            var id =
                reader.IsDBNull(0)
                    ? string.Empty
                    : reader.GetString(0);

            var username =
                reader.IsDBNull(1)
                    ? string.Empty
                    : reader.GetString(1);

            if (
                !string.IsNullOrWhiteSpace(id))
            {
                users[id] =
                    string.IsNullOrWhiteSpace(
                        username)
                        ? id
                        : username;
            }
        }

        return users;
    }


    private static AuditEventDto BuildEvent(
        string table,
        Dictionary<string, object?> row,
        Dictionary<string, string> users)
    {
        var id =
            GetText(
                row,
                "Id",
                "AuditId",
                "LogId");

        if (
            string.IsNullOrWhiteSpace(id))
        {
            id =
                $"{table}-{Guid.NewGuid():N}";
        }

        var userId =
            GetText(
                row,
                "UserId",
                "ActorUserId",
                "AccountId");

        var username =
            !string.IsNullOrWhiteSpace(userId) &&
            users.TryGetValue(
                userId,
                out var resolvedUser)
                ? resolvedUser
                : userId;

        var ipAddress =
            GetText(
                row,
                "IpAddress",
                "IPAddress",
                "Ip",
                "RemoteIpAddress");

        var eventType =
            GetText(
                row,
                "EventType",
                "Type",
                "Action",
                "Result",
                "Status");

        var description =
            GetText(
                row,
                "Description",
                "Message",
                "Details",
                "Reason",
                "FailureReason");

        var userAgent =
            GetText(
                row,
                "UserAgent",
                "Device",
                "DeviceInfo");

        var occurredAt =
            GetDate(
                row,
                "CreatedAt",
                "OccurredAt",
                "Timestamp",
                "LoginAt",
                "LoggedAt",
                "CreatedOn",
                "LoginTime",
                "LastLoginAt");

        var category =
            table switch
            {
                "AuditLogins" =>
                    "login",

                "SecurityLogs" =>
                    "security",

                "LoginHistories" =>
                    "history",

                _ =>
                    "other"
            };

        var status =
            ResolveStatus(row);

        var title =
            !string.IsNullOrWhiteSpace(
                eventType)
                ? eventType
                : table switch
                {
                    "AuditLogins" =>
                        "Login audit",

                    "SecurityLogs" =>
                        "Security event",

                    "LoginHistories" =>
                        "Login history",

                    _ =>
                        "System event"
                };

        var raw =
            new Dictionary<string, string?>(
                StringComparer.OrdinalIgnoreCase);

        foreach (
            var pair in row)
        {
            if (
                IsSensitiveColumn(
                    pair.Key))
            {
                continue;
            }

            raw[pair.Key] =
                ToText(pair.Value);
        }

        return new AuditEventDto
        {
            Id = id,
            Category = category,
            Title = title,
            Description = description,
            UserId = userId,
            Username = username,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            Status = status,
            OccurredAt = occurredAt,
            Source = "REAL SNAPSHOT",
            Table = table,
            Raw = raw
        };
    }


    private static string ResolveStatus(
        Dictionary<string, object?> row)
    {
        var success =
            GetBool(
                row,
                "IsSuccess",
                "Success",
                "Succeeded");

        if (success.HasValue)
        {
            return success.Value
                ? "success"
                : "failed";
        }

        var text =
            string.Join(
                " ",
                new[]
                {
                    GetText(
                        row,
                        "Status"),

                    GetText(
                        row,
                        "Result"),

                    GetText(
                        row,
                        "EventType"),

                    GetText(
                        row,
                        "Type")
                })
                .ToUpperInvariant();

        if (
            text.Contains("FAIL") ||
            text.Contains("ERROR") ||
            text.Contains("DENIED") ||
            text.Contains("BLOCK") ||
            text.Contains("REJECT"))
        {
            return "failed";
        }

        if (
            text.Contains("WARN") ||
            text.Contains("SUSPICIOUS"))
        {
            return "warning";
        }

        if (
            text.Contains("SUCCESS") ||
            text.Contains("OK") ||
            text.Contains("COMPLETED"))
        {
            return "success";
        }

        return "info";
    }


    private static bool? GetBool(
        Dictionary<string, object?> row,
        params string[] names)
    {
        foreach (var name in names)
        {
            if (
                !row.TryGetValue(
                    name,
                    out var value) ||
                value is null)
            {
                continue;
            }

            if (value is bool boolean)
            {
                return boolean;
            }

            if (
                value is byte byteValue)
            {
                return byteValue != 0;
            }

            if (
                value is short shortValue)
            {
                return shortValue != 0;
            }

            if (
                value is int intValue)
            {
                return intValue != 0;
            }

            if (
                bool.TryParse(
                    value.ToString(),
                    out var parsedBool))
            {
                return parsedBool;
            }

            if (
                int.TryParse(
                    value.ToString(),
                    out var parsedInt))
            {
                return parsedInt != 0;
            }
        }

        return null;
    }


    private static string GetText(
        Dictionary<string, object?> row,
        params string[] names)
    {
        foreach (var name in names)
        {
            if (
                row.TryGetValue(
                    name,
                    out var value) &&
                value is not null)
            {
                var text =
                    ToText(value);

                if (
                    !string.IsNullOrWhiteSpace(
                        text))
                {
                    return text!;
                }
            }
        }

        return string.Empty;
    }


    private static string? GetDate(
        Dictionary<string, object?> row,
        params string[] names)
    {
        foreach (var name in names)
        {
            if (
                !row.TryGetValue(
                    name,
                    out var value) ||
                value is null)
            {
                continue;
            }

            if (value is DateTime dateTime)
            {
                if (dateTime.Year <= 1)
                {
                    return null;
                }

                return dateTime.ToString("O");
            }

            if (
                value is
                DateTimeOffset dateTimeOffset)
            {
                if (
                    dateTimeOffset.Year <= 1)
                {
                    return null;
                }

                return dateTimeOffset.ToString(
                    "O");
            }

            if (
                DateTimeOffset.TryParse(
                    value.ToString(),
                    out var parsed))
            {
                if (parsed.Year <= 1)
                {
                    return null;
                }

                return parsed.ToString("O");
            }
        }

        return null;
    }


    private static string? ToText(
        object? value)
    {
        if (value is null)
        {
            return null;
        }

        return value switch
        {
            DateTime dateTime =>
                dateTime.Year <= 1
                    ? null
                    : dateTime.ToString("O"),

            DateTimeOffset dateTimeOffset =>
                dateTimeOffset.Year <= 1
                    ? null
                    : dateTimeOffset.ToString("O"),

            byte[] =>
                "[binary]",

            _ =>
                Convert.ToString(value)
        };
    }


    private static bool IsSensitiveColumn(
        string name)
    {
        var normalized =
            name.ToLowerInvariant();

        return
            normalized.Contains(
                "password") ||
            normalized.Contains(
                "hash") ||
            normalized.Contains(
                "secret") ||
            normalized.Contains(
                "token") ||
            normalized.Contains(
                "otp") ||
            normalized.Contains(
                "securitystamp");
    }


    private sealed class AuditEventDto
    {
        public string Id { get; init; } =
            string.Empty;

        public string Category { get; init; } =
            string.Empty;

        public string Title { get; init; } =
            string.Empty;

        public string Description { get; init; } =
            string.Empty;

        public string UserId { get; init; } =
            string.Empty;

        public string Username { get; init; } =
            string.Empty;

        public string IpAddress { get; init; } =
            string.Empty;

        public string UserAgent { get; init; } =
            string.Empty;

        public string Status { get; init; } =
            "info";

        public string? OccurredAt { get; init; }

        public string Source { get; init; } =
            "REAL SNAPSHOT";

        public string Table { get; init; } =
            string.Empty;

        public Dictionary<string, string?>
            Raw { get; init; } =
                new();
    }
}