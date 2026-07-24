using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class PolicyConfiguration : IEntityTypeConfiguration<Policy>
{
    public void Configure(EntityTypeBuilder<Policy> builder)
    {
        builder.ToTable("Policies");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasConversion(
                   id => id.Value,
                   value => new PolicyId(value));

        builder.Property(x => x.Name);
        builder.Property(x => x.Description);
        builder.Property(x => x.Status);

        builder.OwnsOne(x => x.Amount, b =>
        {
            b.Property(p => p.Amount).HasColumnName("Amount").HasColumnType("decimal(18,2)").IsRequired();
            b.Ignore(p => p.Currency);
        });
    }
}
