using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Policies.Commands.CreatePolicy;

/// <summary>
/// Lệnh tạo mới chính sách.
/// </summary>
/// <param name="Name">Tên chính sách.</param>
/// <param name="Description">Mô tả chính sách.</param>
/// <param name="Amount">Số tiền hỗ trợ.</param>
public sealed record CreatePolicyCommand(
    string Name,
    string Description,
    decimal Amount) : IRequest<Result<Guid>>;
