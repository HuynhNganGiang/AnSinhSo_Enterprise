using System.Collections.Generic;
using AnSinhSo.Application.Map.DTOs;
using MediatR;

namespace AnSinhSo.Application.Map.Queries;

public sealed record GetMapMarkersQuery(
    bool Citizens = true,
    bool Households = true,
    bool PaymentPoints = true,
    bool Welfare = true,
    string? Keyword = null,
    bool? Poor = null,
    bool? NearPoor = null) : IRequest<List<MapMarkerDto>>;
