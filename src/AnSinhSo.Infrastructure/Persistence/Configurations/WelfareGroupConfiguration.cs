using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class WelfareGroupConfiguration : IEntityTypeConfiguration<WelfareGroup>
{
    public void Configure(EntityTypeBuilder<WelfareGroup> builder)
    {
        builder.ToTable("WelfareGroups");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasConversion(
                   id => id.Value,
                   value => new WelfareGroupId(value));

        builder.Property(x => x.Name)
               .HasMaxLength(200)
               .IsRequired();
               
        builder.Property(x => x.Description)
               .HasMaxLength(1000);
               
        builder.Property(x => x.IsActive)
               .IsRequired();
    }
}
