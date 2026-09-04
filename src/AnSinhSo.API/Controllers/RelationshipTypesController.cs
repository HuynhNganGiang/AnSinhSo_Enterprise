using System.Globalization;
using System.Reflection;
using AnSinhSo.Domain.Aggregates.RelationshipTypeAggregate;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers;

/// <summary>
/// ANSINHSO_RELATIONSHIP_TYPES_API_V1
///
/// Read-only lookup endpoint for household relationship types.
/// Production RelationshipTypes are never mutated by this controller.
/// </summary>
[ApiController]
[Route("api/v1/relationship-types")]
public sealed class RelationshipTypesController : ControllerBase
{
    private readonly IRelationshipTypeRepository _repository;

    public RelationshipTypesController(
        IRelationshipTypeRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool activeOnly = false,
        CancellationToken cancellationToken = default)
    {
        var entities =
            await _repository.GetAllAsync(
                cancellationToken);

        var data =
            entities
                .Select(Map)
                .Where(
                    item =>
                        !activeOnly ||
                        item.IsActive)
                .OrderBy(
                    item =>
                        item.Name)
                .ThenBy(
                    item =>
                        item.Code)
                .ToArray();

        return Ok(
            new
            {
                success = true,
                data,
                totalCount = data.Length
            });
    }

    private static RelationshipTypeResponse Map(
        object entity)
    {
        var id =
            ReadId(entity);

        var code =
            ReadText(
                entity,
                "Code",
                "RelationshipCode",
                "MaQuanHe");

        var name =
            ReadText(
                entity,
                "Name",
                "DisplayName",
                "TenQuanHe");

        if (string.IsNullOrWhiteSpace(name))
        {
            name = code;
        }

        var description =
            ReadText(
                entity,
                "Description",
                "MoTa");

        var isActive =
            ReadBoolean(
                entity,
                true,
                "IsActive",
                "Active",
                "Enabled");

        return new RelationshipTypeResponse(
            id,
            code,
            name,
            description,
            isActive,
            "REAL");
    }

    private static string ReadId(
        object source)
    {
        var raw =
            ReadMember(
                source,
                "Id");

        if (raw is null)
        {
            return string.Empty;
        }

        var value =
            ReadMember(
                raw,
                "Value");

        return Convert.ToString(
                   value ?? raw,
                   CultureInfo.InvariantCulture)
               ?? string.Empty;
    }

    private static string ReadText(
        object source,
        params string[] names)
    {
        var raw =
            ReadMember(
                source,
                names);

        if (raw is null)
        {
            return string.Empty;
        }

        if (raw is string text)
        {
            return text;
        }

        var value =
            ReadMember(
                raw,
                "Value");

        return Convert.ToString(
                   value ?? raw,
                   CultureInfo.InvariantCulture)
               ?? string.Empty;
    }

    private static bool ReadBoolean(
        object source,
        bool fallback,
        params string[] names)
    {
        var raw =
            ReadMember(
                source,
                names);

        if (raw is null)
        {
            return fallback;
        }

        if (raw is bool boolean)
        {
            return boolean;
        }

        if (
            bool.TryParse(
                Convert.ToString(
                    raw,
                    CultureInfo.InvariantCulture),
                out var parsed))
        {
            return parsed;
        }

        return fallback;
    }

    private static object? ReadMember(
        object source,
        params string[] names)
    {
        var type =
            source.GetType();

        foreach (var name in names)
        {
            var property =
                type.GetProperty(
                    name,
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.IgnoreCase);

            if (property is not null)
            {
                return property.GetValue(
                    source);
            }

            var field =
                type.GetField(
                    name,
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.IgnoreCase);

            if (field is not null)
            {
                return field.GetValue(
                    source);
            }
        }

        return null;
    }

    private sealed record RelationshipTypeResponse(
        string Id,
        string Code,
        string Name,
        string Description,
        bool IsActive,
        string Source);
}