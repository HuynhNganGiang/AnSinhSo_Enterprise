using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.PaymentAggregate.Events;

/// <summary>
/// Sự kiện miền: Đợt thanh toán bị hủy.
/// </summary>
/// <param name="PaymentId">Định danh của đợt thanh toán bị hủy.</param>
public sealed record PaymentCancelledDomainEvent(
    PaymentId PaymentId) : DomainEvent;

