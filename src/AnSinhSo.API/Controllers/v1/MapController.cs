using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Map.DTOs;
using AnSinhSo.Application.Map.Queries;
using AnSinhSo.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers.v1;

[Route("api/v1/map")]
[Authorize]
public class MapController : ApiControllerBase
{
    private readonly ISender _sender;

    public MapController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Policy = Permissions.Map.View)]
    [ProducesResponseType(typeof(List<MapMarkerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMapMarkers(
        [FromQuery] bool citizens = true,
        [FromQuery] bool households = true,
        [FromQuery] bool paymentPoints = true,
        [FromQuery] bool welfare = true,
        [FromQuery] string? keyword = null,
        [FromQuery] bool? poor = null,
        [FromQuery] bool? nearPoor = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMapMarkersQuery(citizens, households, paymentPoints, welfare, keyword, poor, nearPoor);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("statistics")]
    [Authorize(Policy = Permissions.Map.View)]
    [ProducesResponseType(typeof(MapStatisticsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMapStatistics(CancellationToken cancellationToken = default)
    {
        var query = new GetMapStatisticsQuery();
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("citizens")]
    [Authorize(Policy = Permissions.Map.View)]
    [ProducesResponseType(typeof(List<MapMarkerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCitizensMap(
        [FromQuery] string? keyword = null,
        [FromQuery] bool? poor = null,
        [FromQuery] bool? nearPoor = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMapMarkersQuery(true, false, false, false, keyword, poor, nearPoor);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("households")]
    [Authorize(Policy = Permissions.Map.View)]
    [ProducesResponseType(typeof(List<MapMarkerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHouseholdsMap(
        [FromQuery] string? keyword = null,
        [FromQuery] bool? poor = null,
        [FromQuery] bool? nearPoor = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMapMarkersQuery(false, true, false, false, keyword, poor, nearPoor);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("payment-points")]
    [Authorize(Policy = Permissions.Map.View)]
    [ProducesResponseType(typeof(List<MapMarkerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentPointsMap(
        [FromQuery] string? keyword = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMapMarkersQuery(false, false, true, false, keyword, null, null);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("welfare")]
    [Authorize(Policy = Permissions.Map.View)]
    [ProducesResponseType(typeof(List<MapMarkerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWelfareMap(
        [FromQuery] string? keyword = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMapMarkersQuery(false, false, false, true, keyword, null, null);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }
}
