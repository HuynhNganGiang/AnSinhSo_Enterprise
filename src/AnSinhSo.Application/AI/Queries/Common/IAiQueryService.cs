using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.AI.Queries.GetAiDashboardSummary;

namespace AnSinhSo.Application.AI.Queries.Common;

public interface IAiQueryService
{
    Task<AiDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default);
    Task<List<AnSinhSo.Application.AI.Queries.GetAiRecommendations.AiRecommendationDto>> GetRecommendationsAsync(
        AnSinhSo.Application.AI.Queries.GetAiRecommendations.GetAiRecommendationsQuery query, 
        CancellationToken cancellationToken = default);
}
