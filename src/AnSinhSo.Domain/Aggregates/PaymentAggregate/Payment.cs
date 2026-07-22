using System.Collections.Generic;
using System.Linq;
using AnSinhSo.Domain.Aggregates.PaymentAggregate.BusinessRules;
using AnSinhSo.Domain.Aggregates.PaymentAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.PaymentAggregate.Events;
using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.Guards;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Domain.Aggregates.PaymentAggregate;

/// <summary>
/// Gốc tập hợp (Aggregate Root) đại diện cho đợt thanh toán (Payment).
/// </summary>
public sealed class Payment : AggregateRoot<PaymentId>
{
    private readonly List<PaymentDetail> _details = [];

    /// <summary>
    /// Định danh chính sách liên quan đến đợt thanh toán.
    /// </summary>
    public PolicyId PolicyId { get; private set; }

    /// <summary>
    /// Trạng thái của đợt thanh toán.
    /// </summary>
    public PaymentStatus Status { get; private set; }

    /// <summary>
    /// Danh sách chi tiết thanh toán.
    /// </summary>
    public IReadOnlyCollection<PaymentDetail> Details => _details.AsReadOnly();

    /// <summary>
    /// Constructor ẩn dành cho EF Core.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Payment()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private Payment(
        PaymentId id,
        PolicyId policyId,
        PaymentStatus status) : base(id)
    {
        PolicyId = policyId;
        Status = status;
    }

    /// <summary>
    /// Khởi tạo một Payment mới.
    /// </summary>
    /// <param name="id">Định danh đợt thanh toán.</param>
    /// <param name="policyId">Định danh chính sách.</param>
    /// <returns>Kết quả chứa Payment hoặc lỗi.</returns>
    public static Result<Payment> Create(
        PaymentId id,
        PolicyId policyId)
    {
        Guard.Against.Null(id, nameof(id));
        Guard.Against.Null(policyId, nameof(policyId));

        var payment = new Payment(id, policyId, PaymentStatus.Open);
        payment.RaiseDomainEvent(new PaymentCreatedDomainEvent(id));

        return Result.Success(payment);
    }

    /// <summary>
    /// Thêm một chi tiết thanh toán mới.
    /// </summary>
    /// <param name="detail">Chi tiết thanh toán.</param>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result AddDetail(PaymentDetail detail)
    {
        Guard.Against.Null(detail, nameof(detail));

        if (_details.Any(x => x.Id == detail.Id))
        {
            return Result.Success();
        }

        _details.Add(detail);

        return Result.Success();
    }

    /// <summary>
    /// Đóng đợt thanh toán.
    /// </summary>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result Close()
    {
        if (Status == PaymentStatus.Closed)
        {
            return Result.Success();
        }

        var hasUnpaidDetails = _details.Any(d => d.Status != PaymentDetailStatus.Paid);
        CheckRule(new PaymentCannotCloseWhenUnpaidRule(hasUnpaidDetails));

        Status = PaymentStatus.Closed;
        RaiseDomainEvent(new PaymentClosedDomainEvent(Id));

        return Result.Success();
    }

    /// <summary>
    /// Hủy đợt thanh toán.
    /// </summary>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result Cancel()
    {
        if (Status == PaymentStatus.Cancelled)
        {
            return Result.Success();
        }

        Status = PaymentStatus.Cancelled;
        RaiseDomainEvent(new PaymentCancelledDomainEvent(Id));

        return Result.Success();
    }
}
