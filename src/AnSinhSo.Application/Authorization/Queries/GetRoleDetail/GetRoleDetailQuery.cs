using System;
using AnSinhSo.Application.Authorization.DTOs;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetRoleDetail;

public sealed record GetRoleDetailQuery(Guid RoleId) : IRequest<Result<RoleDetailDto>>;
