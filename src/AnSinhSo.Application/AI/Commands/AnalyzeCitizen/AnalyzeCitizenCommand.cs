using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.AI.Services;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.AI.Commands.AnalyzeCitizen;

public record AnalyzeCitizenCommand(Guid CitizenId) : IRequest<Result>;

public class AnalyzeCitizenCommandHandler : IRequestHandler<AnalyzeCitizenCommand, Result>
{
    private readonly IAiAnalysisService _aiAnalysisService;

    public AnalyzeCitizenCommandHandler(IAiAnalysisService aiAnalysisService)
    {
        _aiAnalysisService = aiAnalysisService;
    }

    public async Task<Result> Handle(AnalyzeCitizenCommand request, CancellationToken cancellationToken)
    {
        await _aiAnalysisService.AnalyzeCitizenAsync(request.CitizenId, cancellationToken);
        return Result.Success();
    }
}
