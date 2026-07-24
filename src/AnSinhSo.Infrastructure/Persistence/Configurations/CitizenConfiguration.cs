using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class CitizenConfiguration : IEntityTypeConfiguration<Citizen>
{
    public void Configure(EntityTypeBuilder<Citizen> builder)
    {
        builder.ToTable("Citizens");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.BirthDate);
        builder.Property(x => x.Gender);
        builder.Property(x => x.Status);
    }
}
