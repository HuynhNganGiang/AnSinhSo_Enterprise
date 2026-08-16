using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.AI.Queries.Common;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.AI.Queries.GetAiRecommendations;

public class GetAiRecommendationsQueryHandler : IRequestHandler<GetAiRecommendationsQuery, Result<List<AiRecommendationDto>>>
{
    private readonly IAiQueryService _aiQueryService;

    public GetAiRecommendationsQueryHandler(IAiQueryService aiQueryService)
    {
        _aiQueryService = aiQueryService;
    }

    public async Task<Result<List<AiRecommendationDto>>> Handle(GetAiRecommendationsQuery request, CancellationToken cancellationToken)
    {
        var result = await _aiQueryService.GetRecommendationsAsync(request, cancellationToken);
        return Result.Success(result);
    }
}
