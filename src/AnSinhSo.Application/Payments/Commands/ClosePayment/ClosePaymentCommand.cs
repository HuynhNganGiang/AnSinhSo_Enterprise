using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Payments.Commands.ClosePayment;

/// <summary>
/// Lệnh đóng đợt chi trả (kết thúc đợt).
/// </summary>
/// <param name="PaymentId">Định danh đợt chi trả.</param>
public sealed record ClosePaymentCommand(Guid PaymentId) : IRequest<Result>;
