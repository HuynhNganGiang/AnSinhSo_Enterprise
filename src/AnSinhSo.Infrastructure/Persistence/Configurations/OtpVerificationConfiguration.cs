using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class OtpVerificationConfiguration : IEntityTypeConfiguration<OtpVerification>
{
    public void Configure(EntityTypeBuilder<OtpVerification> builder)
    {
        builder.ToTable("OtpVerifications");

        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.Id)
            .HasConversion(id => id.Value, value => new OtpVerificationId(value))
            .IsRequired();

        builder.Property(o => o.CitizenIdentityId)
            .HasConversion(id => id.Value, value => CitizenIdentityId.Create(value))
            .IsRequired();

        builder.Property(o => o.CodeHash)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(o => o.TargetPhone)
            .HasConversion(p => p.Value, value => PhoneNumber.Create(value))
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(o => o.ExpiresAt)
            .IsRequired();

        builder.Property(o => o.FailedAttemptCount)
            .IsRequired();

        // Shadow property for Concurrency Token
        builder.Property<byte[]>("RowVersion")
            .IsRowVersion();

        // Indexes
        builder.HasIndex(o => new { o.CitizenIdentityId, o.Status });
    }
}
