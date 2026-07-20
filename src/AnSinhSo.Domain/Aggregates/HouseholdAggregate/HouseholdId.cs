using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate;

/// <summary>
/// Định danh mạnh (Strongly-typed ID) cho Household.
/// </summary>
/// <param name="Value">Giá trị Guid cốt lõi.</param>
public sealed record HouseholdId(Guid Value) : StronglyTypedId<Guid>(Value);
