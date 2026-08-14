using AnSinhSo.Domain.Aggregates.PermissionAggregate;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.Aggregates.RoleAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("RolePermissions");

        // AD #113: Composite PK for RolePermissions
        builder.HasKey(x => new { x.RoleId, x.PermissionId });
        builder.Ignore(x => x.Id); // Ignore the Guid Id from Entity<Guid>

        builder.Property(x => x.RoleId)
            .HasConversion(id => id.Value, value => RoleId.Create(value));
            
        builder.Property(x => x.PermissionId)
            .HasConversion(id => id.Value, value => PermissionId.Create(value));

        // Foreign Key configurations with DeleteBehavior.Restrict
        builder.HasOne<Role>()
            .WithMany(r => r.Permissions)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Permission>()
            .WithMany()
            .HasForeignKey(x => x.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
