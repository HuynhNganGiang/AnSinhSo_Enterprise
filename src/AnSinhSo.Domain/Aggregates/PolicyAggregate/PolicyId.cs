using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.PolicyAggregate;

/// <summary>
/// Ð?nh danh m?nh (Strongly-typed ID) cho Policy.
/// </summary>
/// <param name="Value">Giá tr? Guid c?t lõi.</param>
public sealed record PolicyId(Guid Value) : StronglyTypedId<Guid>(Value);
