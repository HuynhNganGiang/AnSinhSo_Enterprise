using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.AI.Services;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.AI.Commands.ScanAll;

public record ScanAllCommand() : IRequest<Result>;

public class ScanAllCommandHandler : IRequestHandler<ScanAllCommand, Result>
{
    private readonly IAiAnalysisService _aiAnalysisService;

    public ScanAllCommandHandler(IAiAnalysisService aiAnalysisService)
    {
        _aiAnalysisService = aiAnalysisService;
    }

    public async Task<Result> Handle(ScanAllCommand request, CancellationToken cancellationToken)
    {
        await _aiAnalysisService.ScanAllAsync(cancellationToken);
        return Result.Success();
    }
}
