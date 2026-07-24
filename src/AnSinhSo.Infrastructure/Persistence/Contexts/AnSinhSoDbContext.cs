using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Contexts;

public class AnSinhSoDbContext : DbContext
{
    public AnSinhSoDbContext(DbContextOptions<AnSinhSoDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnSinhSoDbContext).Assembly);
    }
}
