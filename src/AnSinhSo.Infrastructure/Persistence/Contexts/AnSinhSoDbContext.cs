using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Contexts;

public class AnSinhSoDbContext : DbContext
{
    public DbSet<Citizen> Citizens { get; set; }
    public DbSet<Household> Households { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Policy> Policies { get; set; }
    public DbSet<WelfareGroup> WelfareGroups { get; set; }

    public AnSinhSoDbContext(DbContextOptions<AnSinhSoDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnSinhSoDbContext).Assembly);
    }
}
