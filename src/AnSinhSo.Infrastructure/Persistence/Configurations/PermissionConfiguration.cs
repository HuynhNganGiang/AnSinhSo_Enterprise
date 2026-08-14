using AnSinhSo.Domain.Aggregates.PermissionAggregate;
using AnSinhSo.Domain.Aggregates.PermissionGroupAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => PermissionId.Create(value))
            .ValueGeneratedNever();

        // Unique Index on Code
        builder.Property(x => x.Code)
            .HasMaxLength(256)
            .IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.PermissionGroupId)
            .HasConversion(
                id => id.Value,
                value => PermissionGroupId.Create(value))
            .IsRequired();
            
        builder.HasOne<PermissionGroup>()
            .WithMany()
            .HasForeignKey(x => x.PermissionGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
