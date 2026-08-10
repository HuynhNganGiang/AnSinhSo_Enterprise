using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations;
using AnSinhSo.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public sealed class CitizenIdentityConfiguration : IEntityTypeConfiguration<CitizenIdentity>
{
    public void Configure(EntityTypeBuilder<CitizenIdentity> builder)
    {
        builder.ToTable("CitizenIdentities");

        // PK
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => CitizenIdentityId.Create(value))
            .ValueGeneratedNever();

        // CitizenId mapping and Unique Index (AD #25)
        builder.Property(x => x.CitizenId)
            .HasConversion(
                id => id.Value,
                value => new CitizenId(value))
            .IsRequired();
        builder.HasIndex(x => x.CitizenId).IsUnique();

        // Status mapping
        builder.Property(x => x.Status)
            .IsRequired();

        // SecurityStamp
        builder.Property(x => x.SecurityStamp)
            .HasMaxLength(256)
            .IsRequired();

        // FailedAttemptCount
        builder.Property(x => x.FailedAttemptCount)
            .IsRequired();

        // PrimaryPhone mapping (Owned Entity via OwnsOne)
        builder.OwnsOne(x => x.PrimaryPhone, phoneBuilder =>
        {
            phoneBuilder.Property(p => p.Value)
                .HasColumnName("PrimaryPhone")
                .HasMaxLength(15)
                .IsRequired();
        });

        // AD #46: Shadow Property for Optimistic Concurrency
        builder.Property<byte[]>("RowVersion")
            .IsRowVersion()
            .IsRequired();

        // AD #44: Map internal collections, AD #43: No AutoInclude
        // LinkedProviders
        builder.OwnsMany(x => x.LinkedProviders, providerBuilder =>
        {
            providerBuilder.ToTable("IdentityLinkedProviders");
            providerBuilder.HasKey(p => p.Id);
            providerBuilder.WithOwner().HasForeignKey("CitizenIdentityId");
            
            providerBuilder.Property(p => p.Id)
                .ValueGeneratedNever();

            providerBuilder.Property(p => p.ProviderType)
                .IsRequired();

            providerBuilder.Property(p => p.SubjectId)
                .HasMaxLength(256)
                .IsRequired();

            providerBuilder.Property(p => p.LinkedAt)
                .IsRequired();
        });
        builder.Navigation(x => x.LinkedProviders).UsePropertyAccessMode(PropertyAccessMode.Field);

        // TrustedDevices
        builder.OwnsMany(x => x.TrustedDevices, deviceBuilder =>
        {
            deviceBuilder.ToTable("IdentityTrustedDevices");
            deviceBuilder.HasKey(d => d.Id);
            deviceBuilder.WithOwner().HasForeignKey("CitizenIdentityId");

            deviceBuilder.Property(d => d.Id)
                .ValueGeneratedNever();

            deviceBuilder.Property(d => d.DeviceId)
                .HasMaxLength(256)
                .IsRequired();

            deviceBuilder.Property(d => d.DeviceName)
                .HasMaxLength(256)
                .IsRequired();

            deviceBuilder.Property(d => d.TrustedAt)
                .IsRequired();
        });
        builder.Navigation(x => x.TrustedDevices).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
