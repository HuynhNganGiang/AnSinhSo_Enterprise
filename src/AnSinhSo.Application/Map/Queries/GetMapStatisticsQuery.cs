using AnSinhSo.Application.Map.DTOs;
using MediatR;

namespace AnSinhSo.Application.Map.Queries;

public sealed record GetMapStatisticsQuery : IRequest<MapStatisticsDto>;
