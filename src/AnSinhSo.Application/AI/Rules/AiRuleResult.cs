using AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;

namespace AnSinhSo.Application.AI.Rules;

public class AiRuleResult
{
    public bool Matched { get; set; }
    public int Score { get; set; }
    public string Reason { get; set; } = string.Empty;
    public AiSeverity Severity { get; set; } = AiSeverity.Info;
}
