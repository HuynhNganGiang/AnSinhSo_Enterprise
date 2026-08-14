using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.WelfareProgramAggregate;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public sealed class WelfareProgramRepository : IWelfareProgramRepository
{
    private readonly AnSinhSoDbContext _context;

    public WelfareProgramRepository(AnSinhSoDbContext context)
    {
        _context = context;
    }

    public async Task<WelfareProgram?> GetByIdAsync(WelfareProgramId id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<WelfareProgram>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<WelfareProgram>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<WelfareProgram>()
            .Where(x => x.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(WelfareProgramId id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<WelfareProgram>().AnyAsync(x => x.Id == id, cancellationToken);
    }

    public void Add(WelfareProgram program)
    {
        _context.Set<WelfareProgram>().Add(program);
    }

    public void Update(WelfareProgram program)
    {
        _context.Set<WelfareProgram>().Update(program);
    }
}
