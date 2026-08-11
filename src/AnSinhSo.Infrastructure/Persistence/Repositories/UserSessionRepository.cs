using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate.ValueObjects;
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

    public Task<UserSession?> GetByIdAsync(UserSessionId id, CancellationToken cancellationToken = default)
    {
        return _context.Set<UserSession>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<UserSession?> GetByRefreshTokenHashAsync(string refreshTokenHash, CancellationToken cancellationToken = default)
    {
        return _context.Set<UserSession>()
            .FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshTokenHash, cancellationToken);
    }

    public async Task<IReadOnlyList<UserSession>> GetActiveSessionsByCitizenAsync(Guid citizenIdentityId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _context.Set<UserSession>()
            .Where(x => x.CitizenIdentityId == citizenIdentityId && !x.IsRevoked && x.ExpiresAt > now)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<UserSession>> GetFamilySessionsAsync(Guid refreshTokenFamilyId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<UserSession>()
            .Where(x => x.RefreshTokenFamilyId == refreshTokenFamilyId)
            .ToListAsync(cancellationToken);
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
