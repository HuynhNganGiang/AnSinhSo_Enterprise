using AnSinhSo.Domain.Aggregates.WelfareProgramAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class WelfareProgramConfiguration : IEntityTypeConfiguration<WelfareProgram>
{
    public void Configure(EntityTypeBuilder<WelfareProgram> builder)
    {
        builder.ToTable("WelfarePrograms");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, value => new WelfareProgramId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.IsActive)
            .IsRequired();

        // Index
        builder.HasIndex(x => x.Code).IsUnique();
    }
}
