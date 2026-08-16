using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.PaymentPointAggregate;

public sealed record PaymentPointId(Guid Value) : StronglyTypedId<Guid>(Value);
