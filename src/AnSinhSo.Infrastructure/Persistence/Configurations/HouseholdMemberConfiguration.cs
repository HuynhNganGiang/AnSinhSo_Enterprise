using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class HouseholdMemberConfiguration : IEntityTypeConfiguration<HouseholdMember>
{
    public void Configure(EntityTypeBuilder<HouseholdMember> builder)
    {
        builder.ToTable("HouseholdMembers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasConversion(
                   id => id.Value,
                   value => new HouseholdMemberId(value));

        builder.Property(x => x.CitizenId)
               .HasConversion(
                   id => id.Value,
                   value => new CitizenId(value))
               .IsRequired();

        builder.Property(x => x.IsHead)
               .IsRequired();
               
        builder.HasIndex("HouseholdId", "CitizenId").IsUnique();
    }
}
