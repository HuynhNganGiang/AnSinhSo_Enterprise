using AnSinhSo.Domain.Aggregates.RelationshipTypeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class RelationshipTypeConfiguration : IEntityTypeConfiguration<RelationshipType>
{
    public void Configure(EntityTypeBuilder<RelationshipType> builder)
    {
        builder.ToTable("RelationshipTypes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasConversion(
                   id => id.Value,
                   value => new RelationshipTypeId(value));

        builder.Property(x => x.Code)
               .IsRequired()
               .HasMaxLength(50);
               
        builder.HasIndex(x => x.Code).IsUnique();

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Description)
               .HasMaxLength(500);

        builder.Property(x => x.IsActive)
               .IsRequired();
    }
}
