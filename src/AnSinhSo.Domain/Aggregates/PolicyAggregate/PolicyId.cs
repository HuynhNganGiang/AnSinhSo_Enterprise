using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.PolicyAggregate;

/// <summary>
/// Định danh mạnh (Strongly-typed ID) cho Policy.
/// </summary>
/// <param name="Value">Giá trị Guid cốt lõi.</param>
public sealed record PolicyId(Guid Value) : StronglyTypedId<Guid>(Value);
