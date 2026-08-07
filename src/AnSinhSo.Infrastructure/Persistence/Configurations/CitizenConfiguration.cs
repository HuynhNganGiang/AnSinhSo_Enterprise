using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AnSinhSo.Infrastructure.Persistence.Converters;
using AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class CitizenConfiguration : IEntityTypeConfiguration<Citizen>
{
    public void Configure(EntityTypeBuilder<Citizen> builder)
    {
        builder.ToTable("Citizens");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasConversion(
                   id => id.Value,
                   value => new CitizenId(value));

        builder.Property(x => x.BirthDate).IsRequired();
        
        builder.Property(x => x.Gender)
               .HasConversion(new EnumerationValueConverter<Gender>())
               .IsRequired();
               
        builder.Property(x => x.Status)
               .HasConversion(new EnumerationValueConverter<CitizenStatus>())
               .IsRequired();

        builder.OwnsOne(x => x.FullName, b =>
        {
            b.Property(p => p.FirstName).HasColumnName("FirstName").HasMaxLength(50).IsRequired();
            b.Property(p => p.MiddleName).HasColumnName("MiddleName").HasMaxLength(50);
            b.Property(p => p.LastName).HasColumnName("LastName").HasMaxLength(50).IsRequired();
        });

        builder.OwnsOne(x => x.CitizenNumber, b =>
        {
            b.Property(p => p.Value).HasColumnName("CitizenNumber").HasMaxLength(12).IsRequired();
            b.HasIndex(p => p.Value).IsUnique();
        });

        builder.OwnsOne(x => x.PhoneNumber, b =>
        {
            b.Property(p => p.Value).HasColumnName("PhoneNumber").HasMaxLength(15).IsRequired();
            b.HasIndex(p => p.Value).IsUnique();
        });

        builder.OwnsOne(x => x.Email, b =>
        {
            b.Property(p => p.Value).HasColumnName("Email").HasMaxLength(100).IsRequired();
            b.HasIndex(p => p.Value).IsUnique();
        });

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
