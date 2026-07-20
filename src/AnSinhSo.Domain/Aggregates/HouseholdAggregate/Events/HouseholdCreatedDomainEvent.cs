using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate.Events;

/// <summary>
/// Sự kiện miền: Hộ gia đình được tạo.
/// </summary>
/// <param name="HouseholdId">Định danh của hộ gia đình được tạo.</param>
public sealed record HouseholdCreatedDomainEvent(
    HouseholdId HouseholdId) : DomainEvent;

