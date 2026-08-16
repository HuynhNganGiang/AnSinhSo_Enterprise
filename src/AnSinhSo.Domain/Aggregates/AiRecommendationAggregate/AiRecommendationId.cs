using System;
using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;

public class AiRecommendationId : ValueObject
{
    public Guid Value { get; private set; }

    private AiRecommendationId() { } // EF Core

    private AiRecommendationId(Guid value)
    {
        Value = value;
    }

    public static AiRecommendationId Create(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("AiRecommendationId cannot be empty");

        return new AiRecommendationId(value);
    }

    public static AiRecommendationId CreateUnique()
    {
        return new AiRecommendationId(Guid.NewGuid());
    }

    protected override System.Collections.Generic.IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
