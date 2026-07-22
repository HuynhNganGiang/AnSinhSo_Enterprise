using System;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.PaymentAggregate.Enumerations;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.Guards;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.PaymentAggregate;

/// <summary>
/// Thực thể (Entity) đại diện cho chi tiết thanh toán của một đối tượng cụ thể.
/// </summary>
public sealed class PaymentDetail : Entity<PaymentDetailId>
{
    /// <summary>
    /// Định danh của công dân (người nhận hỗ trợ).
    /// </summary>
    public CitizenId CitizenId { get; private set; }

    /// <summary>
    /// Số tiền được hỗ trợ.
    /// </summary>
    public Money Amount { get; private set; }

    /// <summary>
    /// Trạng thái của chi tiết thanh toán.
    /// </summary>
    public PaymentDetailStatus Status { get; private set; }

    /// <summary>
    /// Ngày thực tế đã thanh toán (nếu có).
    /// </summary>
    public DateTime? PaidDate { get; private set; }

    /// <summary>
    /// Lý do thất bại (nếu có).
    /// </summary>
    public string? FailureReason { get; private set; }

    /// <summary>
    /// Constructor ẩn dành cho EF Core.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private PaymentDetail()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private PaymentDetail(
        PaymentDetailId id,
        CitizenId citizenId,
        Money amount,
        PaymentDetailStatus status) : base(id)
    {
        CitizenId = citizenId;
        Amount = amount;
        Status = status;
    }

    /// <summary>
    /// Khởi tạo chi tiết thanh toán mới.
    /// </summary>
    /// <param name="id">Định danh chi tiết thanh toán.</param>
    /// <param name="citizenId">Định danh công dân.</param>
    /// <param name="amount">Số tiền.</param>
    /// <returns>Kết quả chứa PaymentDetail hoặc lỗi.</returns>
    public static Result<PaymentDetail> Create(
        PaymentDetailId id,
        CitizenId citizenId,
        Money amount)
    {
        Guard.Against.Null(id, nameof(id));
        Guard.Against.Null(citizenId, nameof(citizenId));
        Guard.Against.Null(amount, nameof(amount));

        var detail = new PaymentDetail(id, citizenId, amount, PaymentDetailStatus.Pending);

        return Result.Success(detail);
    }

    /// <summary>
    /// Đánh dấu là đã thanh toán thành công.
    /// </summary>
    /// <param name="paidDate">Ngày thanh toán.</param>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result MarkPaid(DateTime paidDate)
    {
        if (paidDate == default)
        {
            return Result.Failure(
                Error.Validation(
                    "PaymentDetail.InvalidPaidDate",
                    "Ngày thanh toán không hợp lệ."));
        }

        if (Status == PaymentDetailStatus.Paid)
        {
            return Result.Success();
        }

        Status = PaymentDetailStatus.Paid;
        PaidDate = paidDate;
        FailureReason = null;

        return Result.Success();
    }

    /// <summary>
    /// Đánh dấu là thanh toán thất bại.
    /// </summary>
    /// <param name="reason">Lý do thất bại.</param>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result MarkFailed(string reason)
    {
        Guard.Against.Null(reason, nameof(reason));
        reason = reason.Trim();
        Guard.Against.Empty(reason, nameof(reason));

        if (Status == PaymentDetailStatus.Failed && FailureReason == reason)
        {
            return Result.Success();
        }

        Status = PaymentDetailStatus.Failed;
        FailureReason = reason;
        PaidDate = null;

        return Result.Success();
    }
}
