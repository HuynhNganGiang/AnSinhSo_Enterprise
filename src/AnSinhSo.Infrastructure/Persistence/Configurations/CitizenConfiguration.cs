using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class CitizenConfiguration : IEntityTypeConfiguration<Citizen>
{
    public void Configure(EntityTypeBuilder<Citizen> builder)
    {
        builder.ToTable("Citizens");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.BirthDate);
        builder.Property(x => x.Gender);
        builder.Property(x => x.Status);

        builder.OwnsOne(x => x.FullName, b =>
        {
            b.Property(p => p.FirstName).HasColumnName("FirstName").HasMaxLength(50).IsRequired();
            b.Property(p => p.MiddleName).HasColumnName("MiddleName").HasMaxLength(50);
            b.Property(p => p.LastName).HasColumnName("LastName").HasMaxLength(50).IsRequired();
        });

        builder.OwnsOne(x => x.CitizenNumber, b =>
        {
            b.Property(p => p.Value).HasColumnName("CitizenNumber").HasMaxLength(12).IsRequired();
        });

        builder.OwnsOne(x => x.PhoneNumber, b =>
        {
            b.Property(p => p.Value).HasColumnName("PhoneNumber").HasMaxLength(15).IsRequired();
        });

        builder.OwnsOne(x => x.Email, b =>
        {
            b.Property(p => p.Value).HasColumnName("Email").HasMaxLength(100).IsRequired();
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
