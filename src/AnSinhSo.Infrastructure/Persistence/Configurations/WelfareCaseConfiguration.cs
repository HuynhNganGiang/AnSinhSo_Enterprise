using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;
using AnSinhSo.Domain.Aggregates.WelfareProgramAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class WelfareCaseConfiguration : IEntityTypeConfiguration<WelfareCase>
{
    public void Configure(EntityTypeBuilder<WelfareCase> builder)
    {
        builder.ToTable("WelfareCases");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, value => new WelfareCaseId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.CitizenId)
            .HasConversion(id => id.Value, value => new CitizenId(value))
            .IsRequired();

        builder.Property(x => x.HouseholdId)
            .HasConversion(id => id != null ? id.Value : (System.Guid?)null, value => value.HasValue ? new HouseholdId(value.Value) : null);

        builder.Property(x => x.ProgramId)
            .HasConversion(id => id.Value, value => new WelfareProgramId(value))
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion(status => status.Id, value => WelfareStatus.FromId(value))
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.Property(x => x.BenefitAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.EffectiveFrom);
        builder.Property(x => x.EffectiveTo);

        // Snapshot is an owned type (Complex Type in EF Core 8 is buggy with InMemory provider)
        builder.OwnsOne(x => x.CitizenSnapshot, snapshot =>
        {
            snapshot.Property(s => s.CitizenNumber).HasMaxLength(20).HasColumnName("Snapshot_CitizenNumber");
            snapshot.Property(s => s.FullName).HasMaxLength(255).HasColumnName("Snapshot_FullName");
            snapshot.Property(s => s.DateOfBirth).HasColumnName("Snapshot_DateOfBirth");
            snapshot.Property(s => s.Gender).HasMaxLength(20).HasColumnName("Snapshot_Gender");
            snapshot.Property(s => s.HouseholdCode).HasMaxLength(50).HasColumnName("Snapshot_HouseholdCode");
            snapshot.Property(s => s.Address).HasMaxLength(1000).HasColumnName("Snapshot_Address");
            snapshot.Property(s => s.Phone).HasMaxLength(20).HasColumnName("Snapshot_Phone");
            snapshot.Property(s => s.CreatedAtSnapshot).HasColumnName("Snapshot_CreatedAt");
        });

        builder.HasOne<Citizen>()
            .WithMany()
            .HasForeignKey(x => x.CitizenId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<WelfareProgram>()
            .WithMany()
            .HasForeignKey(x => x.ProgramId)
            .OnDelete(DeleteBehavior.Restrict);

        // Index
        builder.HasIndex(x => x.CitizenId);
        builder.HasIndex(x => x.ProgramId);
    }
}
