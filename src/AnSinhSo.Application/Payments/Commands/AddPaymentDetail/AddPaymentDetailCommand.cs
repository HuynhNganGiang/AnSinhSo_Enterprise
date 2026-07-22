using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Payments.Commands.AddPaymentDetail;

/// <summary>
/// Lệnh thêm chi tiết chi trả (thêm người nhận vào đợt).
/// </summary>
/// <param name="PaymentId">Định danh đợt chi trả.</param>
/// <param name="PaymentDetailId">Định danh chi tiết chi trả.</param>
/// <param name="CitizenId">Định danh công dân nhận hỗ trợ.</param>
/// <param name="Amount">Số tiền hỗ trợ.</param>
public sealed record AddPaymentDetailCommand(
    Guid PaymentId,
    Guid PaymentDetailId,
    Guid CitizenId,
    decimal Amount) : IRequest<Result>;
