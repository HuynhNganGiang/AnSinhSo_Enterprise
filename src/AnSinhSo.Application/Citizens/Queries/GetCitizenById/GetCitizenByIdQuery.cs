using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Citizens.Queries.GetCitizenById;

/// <summary>
/// Truy vấn lấy thông tin chi tiết công dân theo ID.
/// </summary>
/// <param name="CitizenId">Định danh công dân.</param>
public sealed record GetCitizenByIdQuery(Guid CitizenId) : IRequest<Result>;
