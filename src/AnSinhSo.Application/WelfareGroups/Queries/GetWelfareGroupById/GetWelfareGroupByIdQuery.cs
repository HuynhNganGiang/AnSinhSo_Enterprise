using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;
using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.WelfareGroups.Queries.GetWelfareGroupById;

/// <summary>
/// Truy vấn lấy thông tin chi tiết nhóm phúc lợi theo ID.
/// </summary>
/// <param name="WelfareGroupId">Định danh nhóm phúc lợi.</param>
public sealed record GetWelfareGroupByIdQuery(Guid WelfareGroupId) : IRequest<Result>;
