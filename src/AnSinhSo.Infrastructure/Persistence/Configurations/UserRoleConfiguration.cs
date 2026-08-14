using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.RoleAggregate;
using AnSinhSo.Domain.Aggregates.UserRoleAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => UserRoleId.Create(value))
            .ValueGeneratedNever();

        builder.Property(x => x.CitizenIdentityId)
            .HasConversion(
                id => id.Value,
                value => CitizenIdentityId.Create(value))
            .IsRequired();

        builder.Property(x => x.RoleId)
            .HasConversion(
                id => id.Value,
                value => RoleId.Create(value))
            .IsRequired();

        // Composite Unique Index for (CitizenIdentityId, RoleId)
        builder.HasIndex(x => new { x.CitizenIdentityId, x.RoleId }).IsUnique();

        // FK configs
        builder.HasOne<CitizenIdentity>()
            .WithMany()
            .HasForeignKey(x => x.CitizenIdentityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Role>()
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
