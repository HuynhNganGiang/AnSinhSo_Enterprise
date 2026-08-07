using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Policies.Queries.GetPolicyById;

/// <summary>
/// Truy vấn lấy thông tin chi tiết chính sách theo ID.
/// </summary>
/// <param name="PolicyId">Định danh chính sách.</param>
public sealed record GetPolicyByIdQuery(Guid PolicyId) : IRequest<Result>;
