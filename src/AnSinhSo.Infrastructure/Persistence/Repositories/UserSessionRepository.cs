using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public class UserSessionRepository : IUserSessionRepository
{
    private readonly AnSinhSoDbContext _context;

    public UserSessionRepository(AnSinhSoDbContext context)
    {
        _context = context;
    }

    public Task<UserSession?> GetByTokenHashAsync(string tokenHash, CancellationToken ct = default)
    {
        return _context.Set<UserSession>()
            .FirstOrDefaultAsync(x => x.CurrentTokenHash == tokenHash, ct);
    }

    public Task<UserSession?> GetByIdAsync(UserSessionId id, CancellationToken ct = default)
    {
        return _context.Set<UserSession>()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public Task<List<UserSession>> GetActiveSessionsByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        return _context.Set<UserSession>()
            .Where(x => x.UserId.Value == userId && !x.IsRevoked && x.ExpiresAtUtc > now)
            .ToListAsync(ct);
    }

    public Task<List<UserSession>> GetSessionsByFamilyIdAsync(Guid familyId, CancellationToken ct = default)
    {
        return _context.Set<UserSession>()
            .Where(x => x.FamilyId == familyId)
            .ToListAsync(ct);
    }

    public Task<bool> ExistsActiveSessionAsync(Guid userId, string deviceId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        // In EF Core, value objects properties can be queried this way
        return _context.Set<UserSession>()
            .AnyAsync(x => x.UserId.Value == userId && 
                           !x.IsRevoked && 
                           x.ExpiresAtUtc > now && 
                           x.Metadata.DeviceName == deviceId, ct);
    }

    public Task<int> GetActiveSessionCountAsync(Guid userId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        return _context.Set<UserSession>()
            .CountAsync(x => x.UserId.Value == userId && !x.IsRevoked && x.ExpiresAtUtc > now, ct);
    }

    public void Add(UserSession session)
    {
        _context.Set<UserSession>().Add(session);
    }

    public void Update(UserSession session)
    {
        _context.Set<UserSession>().Update(session);
    }
}
