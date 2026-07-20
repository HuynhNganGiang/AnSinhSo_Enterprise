using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate.Events;

/// <summary>
/// Sự kiện miền: Thành viên được thêm vào hộ gia đình.
/// </summary>
/// <param name="HouseholdId">Định danh của hộ gia đình.</param>
/// <param name="CitizenId">Định danh của công dân được thêm.</param>
public sealed record HouseholdMemberAddedDomainEvent(
    HouseholdId HouseholdId,
    CitizenId CitizenId) : DomainEvent;

