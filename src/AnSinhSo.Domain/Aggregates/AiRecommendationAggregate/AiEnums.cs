using System;

namespace AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;

public enum AiTargetType
{
    Citizen = 1,
    Household = 2,
    WelfareCase = 3
}

public enum AiCategory
{
    Suggestion = 1,
    Warning = 2,
    Classification = 3
}

public enum AiConfidence
{
    Low = 1,
    Medium = 2,
    High = 3
}

public enum AiRecommendationStatus
{
    Pending = 1,
    Reviewed = 2,
    Dismissed = 3
}

public enum AiSeverity
{
    Info = 1,
    Low = 2,
    Medium = 3,
    High = 4,
    Critical = 5
}
