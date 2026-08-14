using AnSinhSo.Domain.Aggregates.RoleAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => RoleId.Create(value))
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.IsSystemRole)
            .IsRequired();

        // Map Permissions relation
        builder.HasMany(r => r.Permissions)
            .WithOne()
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Metadata.FindNavigation(nameof(Role.Permissions))?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
