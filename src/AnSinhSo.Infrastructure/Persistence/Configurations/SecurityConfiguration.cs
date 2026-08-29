using AnSinhSo.Domain.Aggregates.SecurityAggregate;
using AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class SecurityLogConfiguration : IEntityTypeConfiguration<SecurityLog>
{
    public void Configure(EntityTypeBuilder<SecurityLog> builder)
    {
        builder.ToTable("SecurityLogs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, v => SecurityLogId.Create(v));
    }
}

public class AuditLoginConfiguration : IEntityTypeConfiguration<AuditLogin>
{
    public void Configure(EntityTypeBuilder<AuditLogin> builder)
    {
        builder.ToTable("AuditLogins");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, v => AuditLoginId.Create(v));
    }
}

public class LoginHistoryConfiguration : IEntityTypeConfiguration<LoginHistory>
{
    public void Configure(EntityTypeBuilder<LoginHistory> builder)
    {
        builder.ToTable("LoginHistories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, v => LoginHistoryId.Create(v));
    }
}

public class DeviceSessionConfiguration : IEntityTypeConfiguration<DeviceSession>
{
    public void Configure(EntityTypeBuilder<DeviceSession> builder)
    {
        builder.ToTable("DeviceSessions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, v => DeviceSessionId.Create(v));
        builder.Property(x => x.Fingerprint).HasMaxLength(256).IsRequired(false);
        builder.Property(x => x.RowVersion).IsRowVersion();

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => new { x.Id, x.RevokedAt, x.IsArchived });
    }
}

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, v => RefreshTokenId.Create(v));
        builder.Property(x => x.DeviceSessionId).HasConversion(x => x.Value, v => DeviceSessionId.Create(v));
        builder.Property(x => x.CitizenIdentityId).IsRequired(false);
        builder.Property(x => x.FamilyId).IsRequired();

        builder.HasIndex(x => x.TokenHash);
        builder.HasIndex(x => x.FamilyId);
        builder.HasIndex(x => x.DeviceSessionId);
    }
}
