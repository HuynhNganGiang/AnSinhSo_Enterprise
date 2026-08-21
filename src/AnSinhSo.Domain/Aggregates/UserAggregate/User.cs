using System;
using System.Collections.Generic;
using System.Linq;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.UserAggregate;

public sealed class User : AggregateRoot<UserId>
{

    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string SecurityStamp { get; private set; } = string.Empty;
    public int AccessFailedCount { get; private set; }
    public DateTime? LockoutEnd { get; private set; }
    public bool IsLocked => LockoutEnd.HasValue && LockoutEnd.Value > DateTime.UtcNow;


    private User() {} // ORM

    // Optional: Constructor to create a new User, but not strictly needed if we only deal with Login here.
    // If we need to create one, we would do it here.
    public static User Create(string username, string email, string passwordHash)
    {
        var user = new User
        {
            Id = new UserId(Guid.NewGuid()),
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            SecurityStamp = Guid.NewGuid().ToString("N")
        };
        return user;
    }


    public void ChangePasswordHash(string newHash)
    {
        PasswordHash = newHash;
        UpdateSecurityStamp();
    }

    public void UpdateSecurityStamp()
    {
        SecurityStamp = Guid.NewGuid().ToString("N");
    }

    public void RecordAccessFailed(int maxFailedAccessAttempts, TimeSpan lockoutTimeSpan)
    {
        AccessFailedCount++;
        if (AccessFailedCount >= maxFailedAccessAttempts)
        {
            LockoutEnd = DateTime.UtcNow.Add(lockoutTimeSpan);
            UpdateSecurityStamp();
        }
    }

    public void ResetAccessFailedCount()
    {
        AccessFailedCount = 0;
        LockoutEnd = null;
    }
}
