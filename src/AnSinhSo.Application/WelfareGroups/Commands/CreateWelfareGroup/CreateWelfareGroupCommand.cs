using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;
using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.WelfareGroups.Commands.CreateWelfareGroup;

/// <summary>
/// Lệnh tạo mới nhóm phúc lợi.
/// </summary>
/// <param name="Name">Tên nhóm phúc lợi.</param>
/// <param name="Description">Mô tả chi tiết nhóm phúc lợi.</param>
public sealed record CreateWelfareGroupCommand(
    string Name,
    string Description) : IRequest<Result<Guid>>;
