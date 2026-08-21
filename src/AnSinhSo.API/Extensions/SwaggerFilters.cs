using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AnSinhSo.Api.Extensions;

public class OrderTagsDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var orderedTags = new List<string>
        {
            "Authentication",
            "Current User",
            "Citizen Identities",
            "Citizens",
            "Households",
            "Policies",
            "WelfareGroups",
            "WelfarePrograms",
            "WelfareCases",
            "Payment Management",
            "Notifications",
            "Map",
            "Ai",
            "Permission Groups",
            "Permissions",
            "Roles",
            "User Roles"
        };

        var tagDescriptions = new Dictionary<string, string>
        {
            { "Authentication", "🔐 Xác thực người dùng" },
            { "Current User", "🙍 Thông tin người dùng hiện tại" },
            { "Citizen Identities", "🪪 Định danh công dân" },
            { "Citizens", "👤 Quản lý Công dân" },
            { "Households", "🏠 Quản lý Hộ gia đình" },
            { "Policies", "📋 Chính sách trợ cấp" },
            { "WelfareGroups", "👪 Nhóm đối tượng" },
            { "WelfarePrograms", "🎯 Chương trình an sinh" },
            { "WelfareCases", "📄 Hồ sơ trợ cấp" },
            { "Payment Management", "💰 Quản lý Chi trả" },
            { "Notifications", "🔔 Trung tâm thông báo" },
            { "Map", "🗺️ Bản đồ GIS" },
            { "Ai", "🤖 Hệ thống AI hỗ trợ ra quyết định" },
            { "Permission Groups", "🔑 Nhóm quyền" },
            { "Permissions", "🔒 Danh sách quyền" },
            { "Roles", "👮 Vai trò" },
            { "User Roles", "👥 Phân quyền người dùng" }
        };

        // First, add missing tags that exist in the operations but not explicitly added to swaggerDoc.Tags
        if (swaggerDoc.Tags == null)
            swaggerDoc.Tags = new List<OpenApiTag>();

        var operationTags = swaggerDoc.Paths.Values
            .SelectMany(p => p.Operations.Values)
            .SelectMany(o => o.Tags)
            .Select(t => t.Name)
            .Distinct();

        foreach (var opTag in operationTags)
        {
            if (!swaggerDoc.Tags.Any(t => t.Name == opTag))
            {
                swaggerDoc.Tags.Add(new OpenApiTag { Name = opTag });
            }
        }

        var tags = swaggerDoc.Tags.OrderBy(t =>
        {
            var index = orderedTags.IndexOf(t.Name);
            return index == -1 ? int.MaxValue : index;
        }).ToList();

        foreach (var tag in tags)
        {
            if (tagDescriptions.TryGetValue(tag.Name, out var description))
            {
                tag.Description = description;
            }
        }

        swaggerDoc.Tags = tags;
    }
}

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum.Clear();
            var enumValues = Enum.GetValues(context.Type);
            var enumDescriptions = new List<string>();
            foreach (var enumValue in enumValues)
            {
                var name = Enum.GetName(context.Type, enumValue);
                if (name != null)
                {
                    schema.Enum.Add(new OpenApiString(name));
                    var value = (int)enumValue;
                    enumDescriptions.Add($"{value} = {name}");
                }
            }
            if (enumDescriptions.Any())
            {
                var existingDesc = string.IsNullOrEmpty(schema.Description) ? "" : schema.Description + "<br/>";
                schema.Description = $"{existingDesc}Mô tả Enum:<br/>{string.Join("<br/>", enumDescriptions)}";
            }
        }
    }
}
