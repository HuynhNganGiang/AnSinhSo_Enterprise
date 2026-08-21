using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate;

/// <summary>
/// Ð?nh danh m?nh (Strongly-typed ID) cho Citizen.
/// </summary>
/// <param name="Value">Giá tr? Guid c?t lõi.</param>
public sealed record CitizenId(Guid Value) : StronglyTypedId<Guid>(Value);
