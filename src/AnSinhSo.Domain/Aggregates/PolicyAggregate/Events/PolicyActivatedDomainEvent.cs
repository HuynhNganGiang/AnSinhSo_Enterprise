using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.PolicyAggregate.Events;

/// <summary>
/// S? ki?n mi?n: Chính sách du?c kích ho?t.
/// </summary>
/// <param name="PolicyId">Ð?nh danh c?a chính sách du?c kích ho?t.</param>
public sealed record PolicyActivatedDomainEvent(
    PolicyId PolicyId) : DomainEvent;

