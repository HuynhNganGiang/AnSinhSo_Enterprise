using AnSinhSo.Application.AI.Rules.Contexts;

namespace AnSinhSo.Application.AI.Rules.Citizens;

public class MissingIdentityRule : IAiRule<AiCitizenContext>
{
    public string RuleCode => "AI-CI-001";
    public string RuleName => "Thiếu thông tin CCCD";
    public int Weight => 20;
    public bool IsEnabled => true;

    public AiRuleResult Evaluate(AiCitizenContext target)
    {
        // Citizen doesn't have an identity number in the basic profile if they haven't been issued one.
        if (string.IsNullOrWhiteSpace(target.Citizen.CitizenNumber?.Value))
        {
            return new AiRuleResult
            {
                Matched = true,
                Score = Weight,
                Reason = "Hồ sơ công dân chưa được cập nhật mã số CCCD/Định danh cá nhân.",
                Severity = AnSinhSo.Domain.Aggregates.AiRecommendationAggregate.AiSeverity.Medium
            };
        }

        return new AiRuleResult { Matched = false };
    }
}
