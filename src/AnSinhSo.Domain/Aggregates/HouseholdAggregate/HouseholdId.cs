using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate;

/// <summary>
/// Ð?nh danh m?nh (Strongly-typed ID) cho Household.
/// </summary>
/// <param name="Value">Giá tr? Guid c?t lõi.</param>
public sealed record HouseholdId(Guid Value) : StronglyTypedId<Guid>(Value);
