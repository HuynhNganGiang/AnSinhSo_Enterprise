using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

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
    public AnSinhSoDbContext(DbContextOptions<AnSinhSoDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnSinhSoDbContext).Assembly);
    }

}
