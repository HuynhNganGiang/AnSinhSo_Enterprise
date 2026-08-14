using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;

namespace AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;

public interface IWelfareCaseRepository
{
    Task<WelfareCase?> GetByIdAsync(WelfareCaseId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsActiveCaseForProgramAsync(CitizenId citizenId, WelfareProgramAggregate.WelfareProgramId programId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WelfareCase>> GetByCitizenIdAsync(CitizenId citizenId, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<WelfareCase> Items, int TotalCount)> SearchAsync(
        string? keyword,
        Guid? programId,
        int? statusId,
        int page,
        int pageSize,
        string? sort,
        CancellationToken cancellationToken = default);

    void Add(WelfareCase welfareCase);
    void Update(WelfareCase welfareCase);
}
