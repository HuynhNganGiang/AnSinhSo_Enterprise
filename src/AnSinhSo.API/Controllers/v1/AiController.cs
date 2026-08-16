using System;
using System.Threading.Tasks;
using AnSinhSo.Application.AI.Commands.AnalyzeCitizen;
using AnSinhSo.Application.AI.Commands.AnalyzeHousehold;
using AnSinhSo.Application.AI.Commands.ScanAll;
using AnSinhSo.Application.AI.Commands.UpdateRecommendationStatus;
using AnSinhSo.Application.AI.Queries.GetAiDashboardSummary;
using AnSinhSo.Application.AI.Queries.GetAiRecommendations;
using AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;
using AnSinhSo.Domain.Constants;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnSinhSo.API.Controllers.V1;

[Route("api/v1/ai")]
[ApiController]
public class AiController : ControllerBase
{
    private readonly ISender _sender;

    public AiController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("analyze-household/{id}")]
    [Authorize(Policy = Permissions.AI.Analyze)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AnalyzeHousehold(Guid id)
    {
        var command = new AnalyzeHouseholdCommand(id);
        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(Result.Success());
    }

    [HttpPost("analyze-citizen/{id}")]
    [Authorize(Policy = Permissions.AI.Analyze)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AnalyzeCitizen(Guid id)
    {
        var command = new AnalyzeCitizenCommand(id);
        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(Result.Success());
    }

    [HttpPost("scan-all")]
    [Authorize(Policy = Permissions.AI.Scan)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ScanAll()
    {
        var command = new ScanAllCommand();
        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(Result.Success());
    }

    [HttpGet("recommendations")]
    [Authorize(Policy = Permissions.AI.ViewRecommendations)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecommendations([FromQuery] AiTargetType? targetType, [FromQuery] AiRecommendationStatus? status)
    {
        var query = new GetAiRecommendationsQuery { TargetType = targetType, Status = status };
        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [HttpGet("dashboard-summary")]
    [Authorize(Policy = Permissions.AI.Dashboard)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboardSummary()
    {
        var query = new GetAiDashboardSummaryQuery();
        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [HttpPatch("recommendations/{id}/status")]
    [Authorize(Policy = Permissions.AI.UpdateStatus)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateAiStatusRequest request)
    {
        var reviewedBy = User.Identity?.Name ?? "System";
        var command = new UpdateRecommendationStatusCommand(id, request.Status, reviewedBy);
        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            if (result.Error.Code.Contains("NotFound"))
            {
                return NotFound(result.Error);
            }
            return BadRequest(result.Error);
        }

        return Ok(Result.Success());
    }
}

public class UpdateAiStatusRequest
{
    public AiRecommendationStatus Status { get; set; }
}
