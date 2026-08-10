using System;
using System.Security.Cryptography;
using System.Text;
using AnSinhSo.Application.Abstractions.Security;

namespace AnSinhSo.Infrastructure.Security;

public sealed class HashProvider : IHashProvider
{
    public string Hash(string input)
    {
        if (string.IsNullOrEmpty(input)) throw new ArgumentNullException(nameof(input));

        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }

    public bool Verify(string input, string hash)
    {
        var inputHash = Hash(input);
        return inputHash == hash;
    }
}
