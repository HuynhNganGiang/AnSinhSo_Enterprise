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


    private User() {} // ORM

    // Optional: Constructor to create a new User, but not strictly needed if we only deal with Login here.
    // If we need to create one, we would do it here.


    public void ChangePasswordHash(string newHash)
    {
        PasswordHash = newHash;
        UpdateSecurityStamp();
    }

    public void UpdateSecurityStamp()
    {
        SecurityStamp = Guid.NewGuid().ToString("N");
    }
}
