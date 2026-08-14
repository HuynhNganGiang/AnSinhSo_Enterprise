using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.PermissionAggregate;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public sealed class PermissionRepository : IPermissionRepository
{
    private readonly AnSinhSoDbContext _context;

    public PermissionRepository(AnSinhSoDbContext context)
    {
        _context = context;
    }

    public void Add(Permission permission)
    {
        _context.Permissions.Add(permission);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.Permissions
            .AnyAsync(p => p.Code == code, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Permission>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Permissions
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Permission?> GetByIdAsync(PermissionId id, CancellationToken cancellationToken = default)
    {
        return await _context.Permissions
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public void Remove(Permission permission)
    {
        _context.Permissions.Remove(permission);
    }

    public void Update(Permission permission)
    {
        _context.Permissions.Update(permission);
    }
}
