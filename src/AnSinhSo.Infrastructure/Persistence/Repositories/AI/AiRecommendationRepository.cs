using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories.AI;

public class AiRecommendationRepository : IAiRecommendationRepository
{
    private readonly AnSinhSoDbContext _dbContext;

    public AiRecommendationRepository(AnSinhSoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AiRecommendation?> GetByIdAsync(AiRecommendationId id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<AiRecommendation>()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<AiRecommendation>> GetByTargetIdAsync(Guid targetId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<AiRecommendation>()
            .Where(r => r.TargetId == targetId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(AiRecommendation recommendation, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<AiRecommendation>().AddAsync(recommendation, cancellationToken);
    }

    public void Update(AiRecommendation recommendation)
    {
        _dbContext.Set<AiRecommendation>().Update(recommendation);
    }
}
