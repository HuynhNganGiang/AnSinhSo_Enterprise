using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate;

/// <summary>
/// Định danh mạnh (Strongly-typed ID) cho Citizen.
/// </summary>
/// <param name="Value">Giá trị Guid cốt lõi.</param>
public sealed record CitizenId(Guid Value) : StronglyTypedId<Guid>(Value);
