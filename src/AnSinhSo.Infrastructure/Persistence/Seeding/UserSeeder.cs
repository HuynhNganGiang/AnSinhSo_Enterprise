using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Common.Security;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using AnSinhSo.Domain.Aggregates.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AnSinhSo.Infrastructure.Persistence.Seeding;

public class UserSeeder : IDataSeeder
{
    public int Order => 5;


    public async Task SeedAsync(AnSinhSoDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        var options = serviceProvider.GetService<IOptions<SeedAdminOptions>>()?.Value;
        if (options == null ||
    string.IsNullOrWhiteSpace(options.Username) ||
    string.IsNullOrWhiteSpace(options.Email) ||
    string.IsNullOrWhiteSpace(options.Password))
{
    throw new InvalidOperationException(
        "SeedAdmin configuration is incomplete.");
}

        var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();

        if (!await context.Users.AnyAsync(u => u.Username == options.Username || u.Email == options.Email, cancellationToken))
        {
            var hash = passwordHasher.Hash(options.Password);
            var user = User.Create(options.Username, options.Email, hash);

            context.Users.Add(user);
        }
    }
}
