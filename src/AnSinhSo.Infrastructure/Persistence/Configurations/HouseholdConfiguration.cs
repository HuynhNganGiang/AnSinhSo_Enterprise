using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AnSinhSo.Infrastructure.Persistence.Converters;
using AnSinhSo.Domain.ValueObjects;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate.Enumerations;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class HouseholdConfiguration : IEntityTypeConfiguration<Household>
{
    public void Configure(EntityTypeBuilder<Household> builder)
    {
        builder.ToTable("Households");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasConversion(
                   id => id.Value,
                   value => new HouseholdId(value));

        builder.Property(x => x.HouseholdCode)
               .HasConversion(
                   code => code.Value,
                   value => new HouseholdCode(value))
               .IsRequired()
               .HasMaxLength(20);

        builder.HasIndex(x => x.HouseholdCode).IsUnique();

        builder.Property(x => x.Status)
               .HasConversion(new EnumerationValueConverter<HouseholdStatus>())
               .IsRequired();

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

        builder.HasMany(x => x.Members)
               .WithOne()
               .HasForeignKey("HouseholdId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Members)
               .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
