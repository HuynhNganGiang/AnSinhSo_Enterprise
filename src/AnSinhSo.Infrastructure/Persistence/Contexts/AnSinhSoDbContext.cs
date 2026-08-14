using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.Aggregates.PermissionAggregate;
using AnSinhSo.Domain.Aggregates.PermissionGroupAggregate;
using AnSinhSo.Domain.Aggregates.UserRoleAggregate;
using AnSinhSo.Domain.Aggregates.RelationshipTypeAggregate;
using AnSinhSo.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace AnSinhSo.Infrastructure.Persistence.Contexts;

public class AnSinhSoDbContext : DbContext
{
    public DbSet<Citizen> Citizens { get; set; }
    public DbSet<Household> Households { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Policy> Policies { get; set; }
    public DbSet<WelfareGroup> WelfareGroups { get; set; }
    public DbSet<AnSinhSo.Domain.Aggregates.UserAggregate.User> Users { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<CitizenIdentity> CitizenIdentities { get; set; }
    public DbSet<OtpVerification> OtpVerifications { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<PermissionGroup> PermissionGroups { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RelationshipType> RelationshipTypes { get; set; }
    public AnSinhSoDbContext(DbContextOptions<AnSinhSoDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnSinhSoDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    var rowVersionProp = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "RowVersion");
                    if (rowVersionProp != null)
                    {
                        rowVersionProp.CurrentValue = System.Guid.NewGuid().ToByteArray().Take(8).ToArray();
                    }
                }
            }
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }

}
