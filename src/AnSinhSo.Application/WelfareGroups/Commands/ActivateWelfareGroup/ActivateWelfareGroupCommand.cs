using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;
using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.WelfareGroups.Commands.ActivateWelfareGroup;

/// <summary>
/// Lệnh kích hoạt nhóm phúc lợi.
/// </summary>
/// <param name="WelfareGroupId">Định danh nhóm phúc lợi.</param>
public sealed record ActivateWelfareGroupCommand(Guid WelfareGroupId) : IRequest<Result>;
