using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AnSinhSo.Infrastructure.Persistence.Converters;
using AnSinhSo.Domain.Aggregates.PolicyAggregate.Enumerations;
using AnSinhSo.Domain.Enumerations;

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

        builder.Property(x => x.Name)
               .HasMaxLength(200)
               .IsRequired();
               
        builder.Property(x => x.Description)
               .HasMaxLength(1000);
               
        builder.Property(x => x.Status)
               .HasConversion(new EnumerationValueConverter<PolicyStatus>())
               .IsRequired();

        builder.OwnsOne(x => x.Amount, b =>
        {
            b.Property(p => p.Amount).HasColumnName("Amount").HasColumnType("decimal(18,2)").IsRequired();
            b.Property(p => p.Currency).HasColumnName("Currency").HasConversion(new EnumerationValueConverter<Currency>()).IsRequired();
        });
    }
}
