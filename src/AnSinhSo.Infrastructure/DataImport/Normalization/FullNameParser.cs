using System;
using System.Linq;

namespace AnSinhSo.Infrastructure.DataImport.Normalization;

public static class FullNameParser
{
    public static (string FirstName, string MiddleName, string LastName) Parse(string? rawName)
    {
        if (string.IsNullOrWhiteSpace(rawName))
            return (string.Empty, string.Empty, string.Empty);

        var parts = rawName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        if (parts.Length == 0)
            return (string.Empty, string.Empty, string.Empty);

        if (parts.Length == 1)
            return (parts[0], string.Empty, string.Empty);

        if (parts.Length == 2)
            return (parts[0], string.Empty, parts[1]);

        var firstName = parts[0];
        var lastName = parts[^1];
        var middleName = string.Join(" ", parts.Skip(1).Take(parts.Length - 2));

        return (firstName, middleName, lastName);
    }
}
