using System;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.WelfareProgramAggregate;

public sealed record WelfareProgramId(Guid Value) : StronglyTypedId<Guid>(Value);
