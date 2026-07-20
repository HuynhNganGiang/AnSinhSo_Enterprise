using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.PaymentAggregate.Events;

/// <summary>
/// Sự kiện miền: Đợt thanh toán bị đóng.
/// </summary>
/// <param name="PaymentId">Định danh của đợt thanh toán.</param>
public sealed record PaymentClosedDomainEvent(
    PaymentId PaymentId) : DomainEvent;

