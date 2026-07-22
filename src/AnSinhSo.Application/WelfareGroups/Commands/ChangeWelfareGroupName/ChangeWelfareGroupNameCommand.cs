using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.WelfareGroups.Commands.ChangeWelfareGroupName;

/// <summary>
/// Lệnh thay đổi tên hoặc mô tả của nhóm phúc lợi.
/// </summary>
/// <param name="WelfareGroupId">Định danh nhóm phúc lợi.</param>
/// <param name="Name">Tên mới của nhóm phúc lợi.</param>
/// <param name="Description">Mô tả mới của nhóm phúc lợi.</param>
public sealed record ChangeWelfareGroupNameCommand(
    Guid WelfareGroupId,
    string Name,
    string Description) : IRequest<Result>;
