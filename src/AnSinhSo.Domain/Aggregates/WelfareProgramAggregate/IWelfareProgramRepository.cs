using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Domain.Aggregates.WelfareProgramAggregate;

public interface IWelfareProgramRepository
{
    Task<WelfareProgram?> GetByIdAsync(WelfareProgramId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WelfareProgram>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(WelfareProgramId id, CancellationToken cancellationToken = default);
    void Add(WelfareProgram program);
    void Update(WelfareProgram program);
}
