using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;
using AnSinhSo.Domain.Aggregates.WelfareProgramAggregate;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public sealed class WelfareCaseRepository : IWelfareCaseRepository
{
    private readonly AnSinhSoDbContext _context;

    public WelfareCaseRepository(AnSinhSoDbContext context)
    {
        _context = context;
    }

    public async Task<WelfareCase?> GetByIdAsync(WelfareCaseId id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<WelfareCase>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsActiveCaseForProgramAsync(CitizenId citizenId, WelfareProgramId programId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<WelfareCase>().AnyAsync(x =>
            x.CitizenId == citizenId &&
            x.ProgramId == programId &&
            x.Status != WelfareStatus.Cancelled &&
            x.Status != WelfareStatus.Closed &&
            x.Status != WelfareStatus.Rejected, cancellationToken);
    }

    public async Task<IReadOnlyList<WelfareCase>> GetByCitizenIdAsync(CitizenId citizenId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<WelfareCase>()
            .Where(x => x.CitizenId == citizenId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<WelfareCase> Items, int TotalCount)> SearchAsync(
        string? keyword,
        Guid? programId,
        int? statusId,
        int page,
        int pageSize,
        string? sort,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<WelfareCase>().AsQueryable();

        if (programId.HasValue)
        {
            var pId = new WelfareProgramId(programId.Value);
            query = query.Where(x => x.ProgramId == pId);
        }

        if (statusId.HasValue)
        {
            var status = WelfareStatus.FromId(statusId.Value);
            query = query.Where(x => x.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.CitizenSnapshot.FullName.Contains(keyword) || x.CitizenSnapshot.CitizenNumber.Contains(keyword));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        // Basic sort by CreatedAt descending if sort is not provided
        if (string.IsNullOrWhiteSpace(sort))
        {
            query = query.OrderByDescending(x => x.CreatedAt);
        }
        else
        {
            // Simple sort handling, in a real application you might want a more dynamic sort
            query = sort.ToLower() switch
            {
                "createdat_asc" => query.OrderBy(x => x.CreatedAt),
                "createdat_desc" => query.OrderByDescending(x => x.CreatedAt),
                _ => query.OrderByDescending(x => x.CreatedAt)
            };
        }

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public void Add(WelfareCase welfareCase)
    {
        _context.Set<WelfareCase>().Add(welfareCase);
    }

    public void Update(WelfareCase welfareCase)
    {
        _context.Set<WelfareCase>().Update(welfareCase);
    }
}
