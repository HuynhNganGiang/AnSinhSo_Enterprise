using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Application.AI.Services;

public interface IAiAnalysisService
{
    Task AnalyzeHouseholdAsync(Guid householdId, CancellationToken cancellationToken = default);
    Task AnalyzeCitizenAsync(Guid citizenId, CancellationToken cancellationToken = default);
    Task ScanAllAsync(CancellationToken cancellationToken = default);
}
