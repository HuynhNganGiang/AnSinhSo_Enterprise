using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.UserAggregate;

public sealed record UserId(Guid Value) : StronglyTypedId<Guid>(Value)
{
    public static UserId CreateUnique() => new(Guid.NewGuid());
}
