using AnSinhSo.Application.Common.Security;
using BCrypt.Net;

namespace AnSinhSo.Infrastructure.Security.Identity;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 12; // Example work factor

    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, WorkFactor);
    }

    public bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }

    public bool NeedsRehash(string hash)
    {
        return BCrypt.Net.BCrypt.PasswordNeedsRehash(hash, WorkFactor);
    }
}
