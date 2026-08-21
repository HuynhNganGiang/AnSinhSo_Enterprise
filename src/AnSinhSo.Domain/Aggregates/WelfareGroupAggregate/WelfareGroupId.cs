using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;

/// <summary>
/// Ð?nh danh m?nh (Strongly-typed ID) cho WelfareGroup.
/// </summary>
/// <param name="Value">Giá tr? Guid c?t lõi.</param>
public sealed record WelfareGroupId(Guid Value) : StronglyTypedId<Guid>(Value);
