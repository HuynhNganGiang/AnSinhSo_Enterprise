using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.AI.Queries.Common;
using AnSinhSo.Application.AI.Queries.GetAiDashboardSummary;
using AnSinhSo.Application.AI.Queries.GetAiRecommendations;
using AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Queries;

public class AiQueryService : IAiQueryService
{
    private readonly AnSinhSoDbContext _dbContext;

    public AiQueryService(AnSinhSoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AiDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
    {
        var recommendations = await _dbContext.Set<AiRecommendation>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var total = recommendations.Count;
        var highRisk = recommendations.Count(r => r.Confidence == AiConfidence.High);
        var mediumRisk = recommendations.Count(r => r.Confidence == AiConfidence.Medium);
        var lowRisk = recommendations.Count(r => r.Confidence == AiConfidence.Low);
        
        var reviewed = recommendations.Count(r => r.Status == AiRecommendationStatus.Reviewed);
        var pending = recommendations.Count(r => r.Status == AiRecommendationStatus.Pending);

        // Compute specific rule hits from Reasons JSON/Collection
        var poorHouseholds = 0;
        var duplicatedBenefits = 0;
        var elderlyWithoutSupport = 0;
        var missingIdentity = 0;

        var ruleCounts = new Dictionary<string, int>();

        foreach (var rec in recommendations)
        {
            foreach (var reason in rec.Reasons)
            {
                if (!string.IsNullOrEmpty(reason.RuleCode))
                {
                    if (ruleCounts.ContainsKey(reason.RuleCode))
                        ruleCounts[reason.RuleCode]++;
                    else
                        ruleCounts[reason.RuleCode] = 1;

                    if (reason.RuleCode == "AI-HH-001") poorHouseholds++;
                    // AI-HH-003 might be duplicated benefits, add tracking later if needed
                    if (reason.RuleCode == "AI-CI-002") elderlyWithoutSupport++;
                    if (reason.RuleCode == "AI-CI-001") missingIdentity++;
                }
            }
        }

        var topRules = ruleCounts
            .OrderByDescending(kvp => kvp.Value)
            .Take(5)
            .Select(kvp => new TopTriggeredRuleDto { RuleCode = kvp.Key, Count = kvp.Value })
            .ToList();

        return new AiDashboardSummaryDto
        {
            TotalRecommendations = total,
            HighRisk = highRisk,
            MediumRisk = mediumRisk,
            LowRisk = lowRisk,
            PoorHouseholds = poorHouseholds,
            DuplicatedBenefits = duplicatedBenefits, // Placeholder for other rule implementation
            ElderlyWithoutSupport = elderlyWithoutSupport,
            MissingIdentity = missingIdentity,
            Reviewed = reviewed,
            Pending = pending,
            TopTriggeredRules = topRules
        };
    }

    public async Task<List<AiRecommendationDto>> GetRecommendationsAsync(GetAiRecommendationsQuery query, CancellationToken cancellationToken = default)
    {
        var dbQuery = _dbContext.Set<AiRecommendation>().AsNoTracking();

        if (query.TargetType.HasValue)
        {
            dbQuery = dbQuery.Where(r => r.TargetType == query.TargetType.Value);
        }

        if (query.Status.HasValue)
        {
            dbQuery = dbQuery.Where(r => r.Status == query.Status.Value);
        }

        var results = await dbQuery
            .OrderByDescending(r => r.Score)
            .ThenByDescending(r => r.CreatedAt)
            .Take(100) // limit for demo
            .ToListAsync(cancellationToken);

        return results.Select(r => new AiRecommendationDto
        {
            Id = r.Id.Value,
            RecommendationNumber = r.RecommendationNumber,
            TargetType = r.TargetType,
            TargetId = r.TargetId,
            Category = r.Category,
            Score = r.Score,
            Confidence = r.Confidence,
            Title = r.Title,
            Summary = r.Summary,
            Status = r.Status,
            CreatedAt = r.CreatedAt,
            Reasons = r.Reasons.Select(rs => new AiReasonDto
            {
                RuleCode = rs.RuleCode,
                Factor = rs.Factor,
                Weight = rs.Weight,
                Message = rs.Message
            }).ToList()
        }).ToList();
    }
}
