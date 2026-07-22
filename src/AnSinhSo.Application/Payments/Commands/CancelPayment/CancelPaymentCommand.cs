using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Payments.Commands.CancelPayment;

/// <summary>
/// Lệnh hủy đợt chi trả.
/// </summary>
/// <param name="PaymentId">Định danh đợt chi trả.</param>
public sealed record CancelPaymentCommand(Guid PaymentId) : IRequest<Result>;
