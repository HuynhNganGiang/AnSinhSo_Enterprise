using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Payments.Commands.CreatePayment;

/// <summary>
/// Lệnh tạo đợt chi trả mới.
/// </summary>
/// <param name="PaymentId">Định danh đợt chi trả.</param>
/// <param name="PolicyId">Định danh chính sách.</param>
public sealed record CreatePaymentCommand(
    Guid PaymentId,
    Guid PolicyId) : IRequest<Result<Guid>>;
