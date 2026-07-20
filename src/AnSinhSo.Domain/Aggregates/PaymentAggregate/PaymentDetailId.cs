using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.PaymentAggregate;

/// <summary>
/// Định danh mạnh (Strongly-typed ID) cho PaymentDetail.
/// </summary>
/// <param name="Value">Giá trị Guid cốt lõi.</param>
public sealed record PaymentDetailId(Guid Value) : StronglyTypedId<Guid>(Value);
