using System;
using System.Text;
using System.Text.RegularExpressions;

namespace AnSinhSo.Infrastructure.DataImport.Normalization;

public interface ICsvStringNormalizer
{
    string? Normalize(string? input);
}

public class CsvStringNormalizer : ICsvStringNormalizer
{
    // Caches regex to normalize whitespace
    private static readonly Regex WhitespaceRegex = new Regex(@"\s+", RegexOptions.Compiled);

    public string? Normalize(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        // 1. Trim
        var normalized = input.Trim();

        // 2. Normalize Unicode (Form C is typical for .NET)
        normalized = normalized.Normalize(NormalizationForm.FormC);

        // 3. Normalize whitespace (collapse multiple spaces to single space)
        normalized = WhitespaceRegex.Replace(normalized, " ");

        // 4. Line ending / carriage returns cleanup within field if any
        normalized = normalized.Replace("\r\n", "\n").Replace("\r", "\n");

        return normalized;
    }
}
