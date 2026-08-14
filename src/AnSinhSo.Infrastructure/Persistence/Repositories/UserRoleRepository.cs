using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.Aggregates.UserRoleAggregate;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public sealed class UserRoleRepository : IUserRoleRepository
{
    private readonly AnSinhSoDbContext _context;

    public UserRoleRepository(AnSinhSoDbContext context)
    {
        _context = context;
    }

    public void Add(UserRole userRole)
    {
        _context.UserRoles.Add(userRole);
    }

    public async Task<bool> ExistsAsync(CitizenIdentityId citizenIdentityId, RoleId roleId, CancellationToken cancellationToken = default)
    {
        return await _context.UserRoles
            .AnyAsync(ur => ur.CitizenIdentityId == citizenIdentityId && ur.RoleId == roleId, cancellationToken);
    }

    public async Task<bool> ExistsByRoleIdAsync(RoleId roleId, CancellationToken cancellationToken = default)
    {
        return await _context.UserRoles
            .AnyAsync(ur => ur.RoleId == roleId, cancellationToken);
    }

    public async Task<UserRole?> GetByCitizenAndRoleAsync(CitizenIdentityId citizenIdentityId, RoleId roleId, CancellationToken cancellationToken = default)
    {
        return await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.CitizenIdentityId == citizenIdentityId && ur.RoleId == roleId, cancellationToken);
    }

    public async Task<UserRole?> GetByIdAsync(UserRoleId id, CancellationToken cancellationToken = default)
    {
        return await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<string>> GetPermissionsByCitizenIdentityIdAsync(CitizenIdentityId id, CancellationToken cancellationToken = default)
    {
        var roleIds = await _context.UserRoles
            .Where(ur => ur.CitizenIdentityId == id)
            .Select(ur => ur.RoleId)
            .ToListAsync(cancellationToken);

        if (roleIds.Count == 0)
        {
            return new List<string>();
        }

        var permissionCodes = await _context.Roles
            .Where(r => roleIds.Contains(r.Id))
            .SelectMany(r => r.Permissions)
            .Join(_context.Permissions, 
                rp => rp.PermissionId, 
                p => p.Id, 
                (rp, p) => p.Code)
            .Distinct()
            .ToListAsync(cancellationToken);

        return permissionCodes;
    }

    public async Task<IReadOnlyCollection<Role>> GetRolesByCitizenIdentityIdAsync(CitizenIdentityId citizenIdentityId, CancellationToken cancellationToken = default)
    {
        var roleIds = await _context.UserRoles
            .Where(ur => ur.CitizenIdentityId == citizenIdentityId)
            .Select(ur => ur.RoleId)
            .ToListAsync(cancellationToken);

        if (roleIds.Count == 0)
        {
            return new List<Role>();
        }

        return await _context.Roles
            .Where(r => roleIds.Contains(r.Id))
            .ToListAsync(cancellationToken);
    }

    public void Remove(UserRole userRole)
    {
        _context.UserRoles.Remove(userRole);
    }
}
