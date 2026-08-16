using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.AI.Services;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.AI.Commands.AnalyzeHousehold;

public record AnalyzeHouseholdCommand(Guid HouseholdId) : IRequest<Result>;

public class AnalyzeHouseholdCommandHandler : IRequestHandler<AnalyzeHouseholdCommand, Result>
{
    private readonly IAiAnalysisService _aiAnalysisService;

    public AnalyzeHouseholdCommandHandler(IAiAnalysisService aiAnalysisService)
    {
        _aiAnalysisService = aiAnalysisService;
    }

    public async Task<Result> Handle(AnalyzeHouseholdCommand request, CancellationToken cancellationToken)
    {
        await _aiAnalysisService.AnalyzeHouseholdAsync(request.HouseholdId, cancellationToken);
        return Result.Success();
    }
}
