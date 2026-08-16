using AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations.AI;

public class AiRecommendationConfiguration : IEntityTypeConfiguration<AiRecommendation>
{
    public void Configure(EntityTypeBuilder<AiRecommendation> builder)
    {
        builder.ToTable("AiRecommendations");

        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => AiRecommendationId.Create(value))
            .ValueGeneratedNever();

        builder.Property(x => x.RecommendationNumber)
            .HasMaxLength(50)
            .IsRequired();
            
        builder.HasIndex(x => x.RecommendationNumber).IsUnique();

        builder.Property(x => x.RuleVersion)
            .HasMaxLength(20);

        builder.Property(x => x.Title)
            .HasMaxLength(200);

        builder.Property(x => x.Summary)
            .HasMaxLength(1000);

        // Store value objects as JSON mapping
        builder.OwnsMany(x => x.Reasons, rb =>
        {
            rb.ToJson(); // EF Core 7+ JSON Column
        });

        builder.Property(x => x.ReviewedBy)
            .HasMaxLength(100);
            
        builder.HasIndex(x => new { x.TargetType, x.TargetId });
    }
}
