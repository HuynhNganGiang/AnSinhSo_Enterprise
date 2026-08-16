using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.AI.Queries.Common;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.AI.Queries.GetAiDashboardSummary;

public class GetAiDashboardSummaryQueryHandler : IRequestHandler<GetAiDashboardSummaryQuery, Result<AiDashboardSummaryDto>>
{
    private readonly IAiQueryService _aiQueryService;

    public GetAiDashboardSummaryQueryHandler(IAiQueryService aiQueryService)
    {
        _aiQueryService = aiQueryService;
    }

    public async Task<Result<AiDashboardSummaryDto>> Handle(GetAiDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        var summary = await _aiQueryService.GetDashboardSummaryAsync(cancellationToken);
        return Result.Success(summary);
    }
}
