using System;
using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;

public class AiReason : ValueObject
{
    public string RuleCode { get; private set; }
    public string Factor { get; private set; }
    public int Weight { get; private set; }
    public string Message { get; private set; }

    private AiReason() 
    { 
        RuleCode = string.Empty;
        Factor = string.Empty;
        Message = string.Empty;
    } // For EF Core

    public AiReason(string ruleCode, string factor, int weight, string message)
    {
        RuleCode = ruleCode;
        Factor = factor;
        Weight = weight;
        Message = message;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return RuleCode;
        yield return Factor;
        yield return Weight;
        yield return Message;
    }
}
