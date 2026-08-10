using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.UserAggregate;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AnSinhSoDbContext _context;

    public UserRepository(AnSinhSoDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .SingleOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }
}
