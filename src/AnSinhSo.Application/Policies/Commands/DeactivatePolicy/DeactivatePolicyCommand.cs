using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Policies.Commands.DeactivatePolicy;

/// <summary>
/// Lệnh hủy kích hoạt chính sách.
/// </summary>
/// <param name="PolicyId">Định danh chính sách.</param>
public sealed record DeactivatePolicyCommand(Guid PolicyId) : IRequest<Result>;
