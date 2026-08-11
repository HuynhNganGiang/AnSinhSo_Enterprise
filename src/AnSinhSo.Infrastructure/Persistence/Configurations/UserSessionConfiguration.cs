using AnSinhSo.Domain.Aggregates.UserSessionAggregate;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("UserSessions");

        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(
                v => v.Value,
                v => new UserSessionId(v))
            .ValueGeneratedNever();

        builder.Property(x => x.CitizenIdentityId)
            .IsRequired();

        builder.Property(x => x.RefreshTokenFamilyId)
            .IsRequired();

        builder.Property(x => x.RefreshTokenHash)
            .HasMaxLength(256)
            .IsRequired();

        builder.OwnsOne(x => x.DeviceInfo, metadata =>
        {
            metadata.Property(m => m.DeviceName)
                .HasColumnName("DeviceName")
                .HasMaxLength(100)
                .IsRequired();
            metadata.Property(m => m.IpAddress)
                .HasColumnName("IpAddress")
                .HasMaxLength(45)
                .IsRequired();
            metadata.Property(m => m.UserAgent)
                .HasColumnName("UserAgent")
                .HasMaxLength(256)
                .IsRequired();
        });

        builder.Property(x => x.ExpiresAt)
            .IsRequired();

        builder.Property(x => x.RevokedAt);

        builder.Property(x => x.RevokeReason)
            .HasMaxLength(200);

        builder.Property(x => x.IsRevoked)
            .IsRequired();

        builder.Property<byte[]>("RowVersion")
            .IsRowVersion()
            .IsRequired();

        // Indexes
        builder.HasIndex(x => x.RefreshTokenHash).IsUnique();
        builder.HasIndex(x => x.RefreshTokenFamilyId);
        
        // Filtered index for user sessions by CitizenIdentityId
        builder.HasIndex(x => new { x.CitizenIdentityId, x.ExpiresAt })
            .HasFilter("[IsRevoked] = 0");
    }
}
