using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public sealed class RoleRepository : IRoleRepository
{
    private readonly AnSinhSoDbContext _context;

    public RoleRepository(AnSinhSoDbContext context)
    {
        _context = context;
    }

    public void Add(Role role)
    {
        _context.Roles.Add(role);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .AnyAsync(r => r.Name == name, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Role?> GetByIdAsync(RoleId id, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public void Remove(Role role)
    {
        _context.Roles.Remove(role);
    }

    public void Update(Role role)
    {
        _context.Roles.Update(role);
    }
}
