using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.PermissionGroupAggregate;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public sealed class PermissionGroupRepository : IPermissionGroupRepository
{
    private readonly AnSinhSoDbContext _context;

    public PermissionGroupRepository(AnSinhSoDbContext context)
    {
        _context = context;
    }

    public void Add(PermissionGroup permissionGroup)
    {
        _context.PermissionGroups.Add(permissionGroup);
    }

    public async Task<IReadOnlyCollection<PermissionGroup>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PermissionGroups
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<PermissionGroup?> GetByIdAsync(PermissionGroupId id, CancellationToken cancellationToken = default)
    {
        return await _context.PermissionGroups
            .FirstOrDefaultAsync(pg => pg.Id == id, cancellationToken);
    }

    public void Remove(PermissionGroup permissionGroup)
    {
        _context.PermissionGroups.Remove(permissionGroup);
    }

    public void Update(PermissionGroup permissionGroup)
    {
        _context.PermissionGroups.Update(permissionGroup);
    }
}
