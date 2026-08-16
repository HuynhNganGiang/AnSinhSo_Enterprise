using System;
using System.Linq;
using AnSinhSo.Application.AI.Rules.Contexts;

namespace AnSinhSo.Application.AI.Rules.Citizens;

public class ElderlyWithoutSupportRule : IAiRule<AiCitizenContext>
{
    public string RuleCode => "AI-CI-002";
    public string RuleName => "Người cao tuổi neo đơn chưa có trợ cấp";
    public int Weight => 50;
    public bool IsEnabled => true;

    public AiRuleResult Evaluate(AiCitizenContext target)
    {
        // Age check uses BirthDate directly since it's not nullable
        var age = (DateTime.UtcNow - target.Citizen.BirthDate).TotalDays / 365.25;

        // Condition 1: >= 65 tuổi
        // Condition 2: Sống neo đơn (không cùng hộ có người < 65) -> in Context
        if (age >= 65 && target.IsLivingAlone && target.WelfareCases.Count == 0)
        {
            return new AiRuleResult
            {
                Matched = true,
                Score = Weight,
                Reason = $"Công dân {Math.Floor(age)} tuổi, sống neo đơn nhưng chưa nhận chính sách trợ cấp nào.",
                Severity = AnSinhSo.Domain.Aggregates.AiRecommendationAggregate.AiSeverity.Critical
            };
        }

        return new AiRuleResult { Matched = false };
    }
}
