using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class HouseholdConfiguration : IEntityTypeConfiguration<Household>
{
    public void Configure(EntityTypeBuilder<Household> builder)
    {
        builder.ToTable("Households");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status);

        builder.OwnsOne(x => x.Address, b =>
        {
            b.Property(p => p.Street).HasColumnName("Street").HasMaxLength(200).IsRequired();
            b.Property(p => p.Ward).HasColumnName("Ward").HasMaxLength(100).IsRequired();
            b.Property(p => p.District).HasColumnName("District").HasMaxLength(100).IsRequired();
            b.Property(p => p.Province).HasColumnName("Province").HasMaxLength(100).IsRequired();
            b.OwnsOne(p => p.PostalCode, pc =>
            {
                pc.Property(p => p.Value).HasColumnName("PostalCode").HasMaxLength(20).IsRequired();
            });
        });
    }
}
