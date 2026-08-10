using System;

namespace AnSinhSo.Domain.Aggregates.UserSessionAggregate;

public readonly record struct UserSessionId(Guid Value)
{
    // Preferred implementation for Guid v7, but falling back to normal Guid for now
    // as Guid.NewGuid() is standard. EF Core Configuration will handle sequential if needed.
    public static UserSessionId New() => new(Guid.NewGuid());
}
