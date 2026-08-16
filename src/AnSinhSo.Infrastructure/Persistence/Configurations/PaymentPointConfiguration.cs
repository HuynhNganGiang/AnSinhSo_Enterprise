using AnSinhSo.Domain.Aggregates.PaymentPointAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AnSinhSo.Infrastructure.Persistence.Converters;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class PaymentPointConfiguration : IEntityTypeConfiguration<PaymentPoint>
{
    public void Configure(EntityTypeBuilder<PaymentPoint> builder)
    {
        builder.ToTable("PaymentPoints");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasConversion(
                   id => id.Value,
                   value => new PaymentPointId(value));

        builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();

        builder.Property(x => x.Description).HasMaxLength(500);

        builder.Property(x => x.DisplayOrder).IsRequired();

        builder.Property(x => x.Status)
               .HasConversion(new EnumerationValueConverter<PaymentPointStatus>())
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

        builder.OwnsOne(x => x.Location, b =>
        {
            b.Property(p => p.Latitude).HasColumnName("Latitude");
            b.Property(p => p.Longitude).HasColumnName("Longitude");
        });
    }
}
