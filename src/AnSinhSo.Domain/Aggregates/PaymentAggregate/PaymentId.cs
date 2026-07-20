using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.PaymentAggregate;

/// <summary>
/// Định danh mạnh (Strongly-typed ID) cho Payment.
/// </summary>
/// <param name="Value">Giá trị Guid cốt lõi.</param>
public sealed record PaymentId(Guid Value) : StronglyTypedId<Guid>(Value);
