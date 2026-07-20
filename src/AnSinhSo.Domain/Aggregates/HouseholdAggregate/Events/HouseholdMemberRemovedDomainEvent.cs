using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate.Events;

/// <summary>
/// Sự kiện miền: Thành viên bị xóa khỏi hộ gia đình.
/// </summary>
/// <param name="HouseholdId">Định danh của hộ gia đình.</param>
/// <param name="CitizenId">Định danh của công dân bị xóa.</param>
public sealed record HouseholdMemberRemovedDomainEvent(
    HouseholdId HouseholdId,
    CitizenId CitizenId) : DomainEvent;

