using System;
using System.Linq;
using AnSinhSo.Application.AI.Rules.Contexts;

namespace AnSinhSo.Application.AI.Rules.Households;

public class VulnerableMembersRule : IAiRule<AiHouseholdContext>
{
    public string RuleCode => "AI-HH-002";
    public string RuleName => "Hộ có nhiều thành viên yếu thế";
    public int Weight => 30;
    public bool IsEnabled => true;

    public AiRuleResult Evaluate(AiHouseholdContext target)
    {
        int vulnerableCount = 0;
        var reasons = new System.Collections.Generic.List<string>();

        foreach (var member in target.Members)
        {
            var age = (DateTime.UtcNow - member.BirthDate).TotalDays / 365.25;

            if (age >= 65 || age <= 6)
            {
                reasons.Add($"{member.FullName} ({age:F0} tuổi)");
                vulnerableCount++;
            }
        }

        var score = vulnerableCount * Weight;

        if (score >= 60) score = 60; // Max score cho tiêu chí này

        if (vulnerableCount > 0)
        {
            return new AiRuleResult
            {
                Matched = true,
                Score = score,
                Reason = "Hộ có thành viên yếu thế: " + string.Join(", ", reasons),
                Severity = AnSinhSo.Domain.Aggregates.AiRecommendationAggregate.AiSeverity.Medium
            };
        }

        return new AiRuleResult { Matched = false };
    }
}
