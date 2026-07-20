using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;

/// <summary>
/// Định danh mạnh (Strongly-typed ID) cho WelfareGroup.
/// </summary>
/// <param name="Value">Giá trị Guid cốt lõi.</param>
public sealed record WelfareGroupId(Guid Value) : StronglyTypedId<Guid>(Value);
