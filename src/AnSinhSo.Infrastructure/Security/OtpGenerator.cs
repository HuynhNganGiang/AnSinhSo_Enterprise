using System;
using System.Security.Cryptography;
using System.Text;
using AnSinhSo.Application.Abstractions.Security;

namespace AnSinhSo.Infrastructure.Security;

public sealed class OtpGenerator : IOtpGenerator
{
    private const string AllowedChars = "0123456789";

    public string Generate(int length = 6)
    {
        if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));

        var sb = new StringBuilder(length);
        using var generator = RandomNumberGenerator.Create();
        var buffer = new byte[1];

        for (int i = 0; i < length; i++)
        {
            generator.GetBytes(buffer);
            var index = buffer[0] % AllowedChars.Length;
            sb.Append(AllowedChars[index]);
        }

        return sb.ToString();
    }
}
