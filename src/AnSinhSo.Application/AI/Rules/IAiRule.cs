namespace AnSinhSo.Application.AI.Rules;

public interface IAiRule<T>
{
    string RuleCode { get; }
    string RuleName { get; }
    int Weight { get; }
    bool IsEnabled { get; }
    AiRuleResult Evaluate(T target);
}
