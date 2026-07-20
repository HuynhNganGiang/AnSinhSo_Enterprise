using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.PaymentAggregate.Events;

/// <summary>
/// Sự kiện miền: Đợt thanh toán được tạo.
/// </summary>
/// <param name="PaymentId">Định danh của đợt thanh toán được tạo.</param>
public sealed record PaymentCreatedDomainEvent(
    PaymentId PaymentId) : DomainEvent;

