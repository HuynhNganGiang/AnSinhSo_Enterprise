using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.AI.Rules;
using AnSinhSo.Application.AI.Rules.Contexts;
using AnSinhSo.Application.AI.Services;
using AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AnSinhSo.Infrastructure.Services.AI;

public class AiAnalysisService : IAiAnalysisService
{
    private const string RuleVersion = "1.0";

    private readonly AnSinhSoDbContext _dbContext;
    private readonly IEnumerable<IAiRule<AiHouseholdContext>> _householdRules;
    private readonly IEnumerable<IAiRule<AiCitizenContext>> _citizenRules;
    private readonly ILogger<AiAnalysisService> _logger;

    public AiAnalysisService(
        AnSinhSoDbContext dbContext,
        IEnumerable<IAiRule<AiHouseholdContext>> householdRules,
        IEnumerable<IAiRule<AiCitizenContext>> citizenRules,
        ILogger<AiAnalysisService> logger)
    {
        _dbContext = dbContext;
        _householdRules = householdRules;
        _citizenRules = citizenRules;
        _logger = logger;
    }

    public async Task AnalyzeHouseholdAsync(Guid householdId, CancellationToken cancellationToken = default)
    {
        var context = await BuildHouseholdContextAsync(householdId, cancellationToken);
        if (context == null) return;

        await RunHouseholdRulesAsync(context, cancellationToken);
    }

    public async Task AnalyzeCitizenAsync(Guid citizenId, CancellationToken cancellationToken = default)
    {
        var context = await BuildCitizenContextAsync(citizenId, cancellationToken);
        if (context == null) return;

        await RunCitizenRulesAsync(context, cancellationToken);
    }

    public async Task ScanAllAsync(CancellationToken cancellationToken = default)
    {
        // 1. Scan all active households
        var households = await _dbContext.Households
            .Where(h => h.Status == HouseholdStatus.Active)
            .Select(h => h.Id.Value)
            .ToListAsync(cancellationToken);

        foreach (var householdId in households)
        {
            await AnalyzeHouseholdAsync(householdId, cancellationToken);
        }

        // 2. Scan all active citizens
        var citizens = await _dbContext.Citizens
            .Where(c => c.Status == CitizenStatus.Active)
            .Select(c => c.Id.Value)
            .ToListAsync(cancellationToken);

        foreach (var citizenId in citizens)
        {
            await AnalyzeCitizenAsync(citizenId, cancellationToken);
        }
    }

    private async Task<AiHouseholdContext?> BuildHouseholdContextAsync(Guid householdId, CancellationToken cancellationToken)
    {
        var householdIdObj = new HouseholdId(householdId);
        var household = await _dbContext.Households
            .Include(h => h.Members)
            .FirstOrDefaultAsync(h => h.Id == householdIdObj, cancellationToken);

        if (household == null) return null;

        var memberCitizenIds = household.Members.Select(m => m.CitizenId).ToList();
        var citizens = await _dbContext.Citizens
            .Where(c => memberCitizenIds.Contains(c.Id))
            .ToListAsync(cancellationToken);

        var welfareCases = await _dbContext.Set<AnSinhSo.Domain.Aggregates.WelfareCaseAggregate.WelfareCase>()
            .Where(w => w.HouseholdId == householdIdObj)
            .ToListAsync(cancellationToken);

        return new AiHouseholdContext
        {
            Household = household,
            Members = citizens,
            WelfareCases = welfareCases,
            TotalEstimatedMonthlyIncome = 0 // Normally queried from some financial records, left as 0 for rules to trigger for demo
        };
    }

    private async Task<AiCitizenContext?> BuildCitizenContextAsync(Guid citizenId, CancellationToken cancellationToken)
    {
        var citizenIdObj = new CitizenId(citizenId);
        var citizen = await _dbContext.Citizens
            .FirstOrDefaultAsync(c => c.Id == citizenIdObj, cancellationToken);

        if (citizen == null) return null;

        var welfareCases = await _dbContext.Set<AnSinhSo.Domain.Aggregates.WelfareCaseAggregate.WelfareCase>()
            .Where(w => w.CitizenId == citizenIdObj)
            .ToListAsync(cancellationToken);

        var memberOfHousehold = await _dbContext.Households
            .AnyAsync(h => h.Members.Any(m => m.CitizenId == citizenIdObj) && h.Members.Count > 1, cancellationToken);

        return new AiCitizenContext
        {
            Citizen = citizen,
            IsLivingAlone = !memberOfHousehold,
            WelfareCases = welfareCases
        };
    }

    private async Task RunHouseholdRulesAsync(AiHouseholdContext context, CancellationToken cancellationToken)
    {
        var totalScore = 0;
        var reasons = new List<AiReason>();
        var highestSeverity = AiSeverity.Info;

        foreach (var rule in _householdRules.Where(r => r.IsEnabled))
        {
            var result = rule.Evaluate(context);
            if (result.Matched)
            {
                totalScore += result.Score;
                reasons.Add(new AiReason(rule.RuleCode, rule.RuleName, result.Score, result.Reason));
                
                if (result.Severity > highestSeverity)
                {
                    highestSeverity = result.Severity;
                }
            }
        }

        if (totalScore > 0)
        {
            await SaveRecommendationAsync(
                AiTargetType.Household,
                context.Household.Id.Value,
                AiCategory.Suggestion, // Derive dynamically if needed
                totalScore,
                highestSeverity,
                "Có khả năng thuộc diện cần quan tâm (Household)",
                "AI phát hiện hộ gia đình có dấu hiệu cần được quan tâm hoặc hỗ trợ chính sách.",
                reasons,
                cancellationToken);
        }
    }

    private async Task RunCitizenRulesAsync(AiCitizenContext context, CancellationToken cancellationToken)
    {
        var totalScore = 0;
        var reasons = new List<AiReason>();
        var highestSeverity = AiSeverity.Info;

        foreach (var rule in _citizenRules.Where(r => r.IsEnabled))
        {
            var result = rule.Evaluate(context);
            if (result.Matched)
            {
                totalScore += result.Score;
                reasons.Add(new AiReason(rule.RuleCode, rule.RuleName, result.Score, result.Reason));
                
                if (result.Severity > highestSeverity)
                {
                    highestSeverity = result.Severity;
                }
            }
        }

        if (totalScore > 0)
        {
            await SaveRecommendationAsync(
                AiTargetType.Citizen,
                context.Citizen.Id.Value,
                AiCategory.Warning, // Derive dynamically if needed
                totalScore,
                highestSeverity,
                "Có khả năng cần bổ sung thông tin hoặc hỗ trợ (Citizen)",
                "AI phát hiện công dân có dấu hiệu cần cập nhật dữ liệu hoặc cấp chính sách.",
                reasons,
                cancellationToken);
        }
    }

    private async Task SaveRecommendationAsync(
        AiTargetType targetType,
        Guid targetId,
        AiCategory category,
        int score,
        AiSeverity highestSeverity,
        string title,
        string summary,
        List<AiReason> reasons,
        CancellationToken cancellationToken)
    {
        var confidence = score >= 80 ? AiConfidence.High : (score >= 40 ? AiConfidence.Medium : AiConfidence.Low);
        
        // Remove existing pending recommendations for this target to avoid spam
        var existing = await _dbContext.Set<AiRecommendation>()
            .Where(r => r.TargetType == targetType && r.TargetId == targetId && r.Status == AiRecommendationStatus.Pending)
            .ToListAsync(cancellationToken);
            
        _dbContext.Set<AiRecommendation>().RemoveRange(existing);

        var datePrefix = DateTime.UtcNow.ToString("yyyyMMdd");
        // Count today's records for sequence. In production, use a Sequence or better generator.
        var todayCount = await _dbContext.Set<AiRecommendation>()
            .CountAsync(r => r.RecommendationNumber.StartsWith($"AI-{datePrefix}"), cancellationToken);
        
        var sequence = (todayCount + 1).ToString("D6");
        var recommendationNumber = $"AI-{datePrefix}-{sequence}";

        var recommendation = AiRecommendation.Create(
            recommendationNumber,
            targetType,
            targetId,
            RuleVersion,
            category,
            score,
            confidence,
            title,
            summary,
            reasons);

        await _dbContext.Set<AiRecommendation>().AddAsync(recommendation, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
