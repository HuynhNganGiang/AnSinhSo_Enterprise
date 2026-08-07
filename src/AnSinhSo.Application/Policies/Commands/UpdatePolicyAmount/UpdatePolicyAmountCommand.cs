using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Policies.Commands.UpdatePolicyAmount;

/// <summary>
/// Lệnh cập nhật số tiền hỗ trợ của chính sách.
/// </summary>
/// <param name="PolicyId">Định danh chính sách.</param>
/// <param name="Amount">Số tiền hỗ trợ mới.</param>
public sealed record UpdatePolicyAmountCommand(
    Guid PolicyId,
    decimal Amount) : IRequest<Result>;
