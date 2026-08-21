using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate.Events;

/// <summary>
/// S? ki?n mi?n: Thành viên du?c thêm vào h? gia dình.
/// </summary>
/// <param name="HouseholdId">Ð?nh danh c?a h? gia dình.</param>
/// <param name="CitizenId">Ð?nh danh c?a công dân du?c thêm.</param>
public sealed record HouseholdMemberAddedDomainEvent(
    HouseholdId HouseholdId,
    CitizenId CitizenId) : DomainEvent;

