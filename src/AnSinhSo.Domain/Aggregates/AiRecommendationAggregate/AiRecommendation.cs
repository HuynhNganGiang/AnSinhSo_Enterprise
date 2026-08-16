using System;
using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;

public class AiRecommendation : AggregateRoot<AiRecommendationId>
{
    public string RecommendationNumber { get; private set; }
    public AiTargetType TargetType { get; private set; }
    public Guid TargetId { get; private set; }
    public string RuleVersion { get; private set; }
    public AiCategory Category { get; private set; }
    public int Score { get; private set; }
    public AiConfidence Confidence { get; private set; }
    public string Title { get; private set; }
    public string Summary { get; private set; }
    public IReadOnlyList<AiReason> Reasons => _reasons.AsReadOnly();
    public AiRecommendationStatus Status { get; private set; }
    public string? ReviewedBy { get; private set; }
    public DateTime? ReviewedAt { get; private set; }

    private readonly List<AiReason> _reasons = new();

    private AiRecommendation() 
    { 
        RecommendationNumber = string.Empty;
        RuleVersion = string.Empty;
        Title = string.Empty;
        Summary = string.Empty;
    } // For EF Core

    public static AiRecommendation Create(
        string recommendationNumber,
        AiTargetType targetType,
        Guid targetId,
        string ruleVersion,
        AiCategory category,
        int score,
        AiConfidence confidence,
        string title,
        string summary,
        List<AiReason> reasons)
    {
        var recommendation = new AiRecommendation
        {
            Id = AiRecommendationId.CreateUnique(),
            RecommendationNumber = recommendationNumber,
            TargetType = targetType,
            TargetId = targetId,
            RuleVersion = ruleVersion,
            Category = category,
            Score = score,
            Confidence = confidence,
            Title = title,
            Summary = summary,
            Status = AiRecommendationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        if (reasons != null)
        {
            recommendation._reasons.AddRange(reasons);
        }

        return recommendation;
    }

    public void MarkAsReviewed(string reviewedBy)
    {
        if (Status != AiRecommendationStatus.Pending)
            throw new InvalidOperationException("Can only review pending recommendations.");

        Status = AiRecommendationStatus.Reviewed;
        ReviewedBy = reviewedBy;
        ReviewedAt = DateTime.UtcNow;
    }

    public void Dismiss(string reviewedBy)
    {
        if (Status != AiRecommendationStatus.Pending)
            throw new InvalidOperationException("Can only dismiss pending recommendations.");

        Status = AiRecommendationStatus.Dismissed;
        ReviewedBy = reviewedBy;
        ReviewedAt = DateTime.UtcNow;
    }
}
