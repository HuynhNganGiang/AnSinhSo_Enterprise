using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.WelfareGroups.Commands.DeactivateWelfareGroup;

/// <summary>
/// Lệnh hủy kích hoạt nhóm phúc lợi.
/// </summary>
/// <param name="WelfareGroupId">Định danh nhóm phúc lợi.</param>
public sealed record DeactivateWelfareGroupCommand(Guid WelfareGroupId) : IRequest<Result>;
