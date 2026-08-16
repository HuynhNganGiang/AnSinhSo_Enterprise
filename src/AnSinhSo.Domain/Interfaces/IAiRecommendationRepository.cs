using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;

namespace AnSinhSo.Domain.Interfaces;

public interface IAiRecommendationRepository
{
    Task<AiRecommendation?> GetByIdAsync(AiRecommendationId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AiRecommendation>> GetByTargetIdAsync(Guid targetId, CancellationToken cancellationToken = default);
    Task AddAsync(AiRecommendation recommendation, CancellationToken cancellationToken = default);
    void Update(AiRecommendation recommendation);
}
