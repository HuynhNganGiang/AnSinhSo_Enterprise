using AnSinhSo.Domain.Aggregates.ZaloUserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class ZaloUserConfiguration : IEntityTypeConfiguration<ZaloUser>
{
    public void Configure(EntityTypeBuilder<ZaloUser> builder)
    {
        builder.ToTable("ZaloUsers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ZaloId)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.ZaloId)
            .IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Avatar)
            .HasMaxLength(1000);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20);

        builder.HasIndex(x => x.CitizenId);
        builder.HasIndex(x => x.CitizenIdentityId);

        // Link with Citizen and CitizenIdentity (Assuming Citizen and CitizenIdentity exist)
        // Note: No navigation property explicitly set on ZaloUser to avoid complexity unless needed.
    }
}
