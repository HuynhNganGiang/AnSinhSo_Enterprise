using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.UserAggregate;

namespace AnSinhSo.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    void Update(User user);
}
