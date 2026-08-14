using System;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;

public sealed record WelfareCaseId(Guid Value) : StronglyTypedId<Guid>(Value);
