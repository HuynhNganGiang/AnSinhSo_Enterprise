using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.RelationshipTypeAggregate;

/// <summary>
/// Định danh mạnh (Strongly-typed ID) cho RelationshipType.
/// </summary>
public sealed record RelationshipTypeId(Guid Value) : StronglyTypedId<Guid>(Value);
