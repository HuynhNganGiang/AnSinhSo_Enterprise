using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.ZaloUserAggregate;
using AnSinhSo.Domain.Interfaces.Repositories;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public class ZaloUserRepository : IZaloUserRepository
{
    private readonly AnSinhSoDbContext _context;

    public ZaloUserRepository(AnSinhSoDbContext context)
    {
        _context = context;
    }

    public async Task<ZaloUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ZaloUsers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<ZaloUser?> GetByZaloIdAsync(string zaloId, CancellationToken cancellationToken = default)
    {
        return await _context.ZaloUsers.FirstOrDefaultAsync(x => x.ZaloId == zaloId, cancellationToken);
    }

    public async Task<ZaloUser?> GetByCitizenIdAsync(Guid citizenId, CancellationToken cancellationToken = default)
    {
        return await _context.ZaloUsers.FirstOrDefaultAsync(x => x.CitizenId == citizenId, cancellationToken);
    }

    public async Task<ZaloUser?> GetByCitizenIdentityIdAsync(Guid citizenIdentityId, CancellationToken cancellationToken = default)
    {
        return await _context.ZaloUsers.FirstOrDefaultAsync(x => x.CitizenIdentityId == citizenIdentityId, cancellationToken);
    }

    public void Add(ZaloUser zaloUser)
    {
        _context.ZaloUsers.Add(zaloUser);
    }

    public void Update(ZaloUser zaloUser)
    {
        _context.ZaloUsers.Update(zaloUser);
    }
}
