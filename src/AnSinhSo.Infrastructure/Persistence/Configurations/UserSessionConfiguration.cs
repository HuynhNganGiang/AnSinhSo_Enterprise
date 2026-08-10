using AnSinhSo.Domain.Aggregates.UserAggregate;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate;
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
            .ValueGeneratedNever(); // Using Guid.NewGuid() for now, which is handled at domain level.
        builder.Property(x => x.UserId)
            .HasConversion(
                v => v.Value,
                v => new UserId(v))
            .IsRequired();

        // User is not mapped in this DbContext, so we just store the UserId.

        builder.Property(x => x.FamilyId)
            .IsRequired();

        builder.Property(x => x.CurrentTokenHash)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.SecurityStampSnapshot)
            .HasMaxLength(256)
            .IsRequired();

        builder.OwnsOne(x => x.Metadata, metadata =>
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


        builder.Property(x => x.LastActivityUtc)
            .IsRequired();

        builder.Property(x => x.ExpiresAtUtc)
            .IsRequired();

        builder.Property(x => x.RevokedAtUtc);

        builder.Property(x => x.RevokedReason)
            .HasMaxLength(200);

        builder.Property(x => x.IsRevoked)
            .IsRequired();

        builder.Property(x => x.RowVersion)
            .IsRowVersion()
            .IsRequired();

        // Indexes
        builder.HasIndex(x => x.CurrentTokenHash).IsUnique();
        builder.HasIndex(x => x.FamilyId);
        
        // Filtered index for user sessions
        builder.HasIndex(x => new { x.UserId, x.ExpiresAtUtc })
            .HasFilter("[IsRevoked] = 0");
    }
}
