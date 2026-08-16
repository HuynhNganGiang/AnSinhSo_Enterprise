using System.Linq;
using AnSinhSo.Application.AI.Rules.Contexts;

namespace AnSinhSo.Application.AI.Rules.Households;

public class LowIncomeRule : IAiRule<AiHouseholdContext>
{
    public string RuleCode => "AI-HH-001";
    public string RuleName => "Thu nhập dưới chuẩn nghèo";
    public int Weight => 40;
    public bool IsEnabled => true;

    private const decimal POVERTY_LINE_PER_PERSON = 1500000; // 1.5 triệu/người/tháng

    public AiRuleResult Evaluate(AiHouseholdContext target)
    {
        if (target.Members.Count == 0)
            return new AiRuleResult { Matched = false };

        var incomePerPerson = target.TotalEstimatedMonthlyIncome / target.Members.Count;

        if (incomePerPerson <= POVERTY_LINE_PER_PERSON)
        {
            return new AiRuleResult
            {
                Matched = true,
                Score = Weight,
                Reason = $"Thu nhập bình quân đầu người ({incomePerPerson:N0}đ) dưới chuẩn nghèo ({POVERTY_LINE_PER_PERSON:N0}đ).",
                Severity = AnSinhSo.Domain.Aggregates.AiRecommendationAggregate.AiSeverity.High
            };
        }

        return new AiRuleResult { Matched = false };
    }
}
