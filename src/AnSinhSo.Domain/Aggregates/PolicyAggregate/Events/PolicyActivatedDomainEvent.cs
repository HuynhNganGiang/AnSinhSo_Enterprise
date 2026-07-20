using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.PolicyAggregate.Events;

/// <summary>
/// Sự kiện miền: Chính sách được kích hoạt.
/// </summary>
/// <param name="PolicyId">Định danh của chính sách được kích hoạt.</param>
public sealed record PolicyActivatedDomainEvent(
    PolicyId PolicyId) : DomainEvent;

