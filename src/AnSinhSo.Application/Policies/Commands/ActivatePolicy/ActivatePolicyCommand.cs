using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Policies.Commands.ActivatePolicy;

/// <summary>
/// Lệnh kích hoạt chính sách.
/// </summary>
/// <param name="PolicyId">Định danh chính sách.</param>
public sealed record ActivatePolicyCommand(Guid PolicyId) : IRequest<Result>;
