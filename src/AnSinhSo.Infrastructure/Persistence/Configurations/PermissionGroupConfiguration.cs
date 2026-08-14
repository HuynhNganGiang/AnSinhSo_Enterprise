using AnSinhSo.Domain.Aggregates.PermissionGroupAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public sealed class PermissionGroupConfiguration : IEntityTypeConfiguration<PermissionGroup>
{
    public void Configure(EntityTypeBuilder<PermissionGroup> builder)
    {
        builder.ToTable("PermissionGroups");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => PermissionGroupId.Create(value))
            .ValueGeneratedNever();

        builder.Property(x => x.Code)
            .HasMaxLength(256)
            .IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);
    }
}
