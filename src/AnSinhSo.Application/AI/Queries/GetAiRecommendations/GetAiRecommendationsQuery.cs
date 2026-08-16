using System;
using System.Collections.Generic;
using AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.AI.Queries.GetAiRecommendations;

public class GetAiRecommendationsQuery : IRequest<Result<List<AiRecommendationDto>>>
{
    public AiTargetType? TargetType { get; set; }
    public AiRecommendationStatus? Status { get; set; }
}

public class AiRecommendationDto
{
    public Guid Id { get; set; }
    public string RecommendationNumber { get; set; } = string.Empty;
    public AiTargetType TargetType { get; set; }
    public Guid TargetId { get; set; }
    public AiCategory Category { get; set; }
    public int Score { get; set; }
    public AiConfidence Confidence { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public AiRecommendationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<AiReasonDto> Reasons { get; set; } = new();
}

public class AiReasonDto
{
    public string RuleCode { get; set; } = string.Empty;
    public string Factor { get; set; } = string.Empty;
    public int Weight { get; set; }
    public string Message { get; set; } = string.Empty;
}
