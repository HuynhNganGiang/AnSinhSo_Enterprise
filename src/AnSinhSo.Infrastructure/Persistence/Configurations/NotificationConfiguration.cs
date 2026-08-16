using AnSinhSo.Domain.Aggregates.NotificationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, value => NotificationId.Create(value));

        builder.Property(x => x.RecipientCitizenId)
            .HasConversion(id => id.Value, value => new AnSinhSo.Domain.Aggregates.CitizenAggregate.CitizenId(value))
            .IsRequired();

        builder.Property(x => x.Channel).HasConversion<string>();
        builder.Property(x => x.Priority).HasConversion<string>();
        builder.Property(x => x.Status).HasConversion<string>();
        builder.Property(x => x.SourceModule).HasConversion<string>();

        builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Content).IsRequired();

        builder.HasMany(x => x.Timeline)
            .WithOne()
            .HasForeignKey(x => x.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.Metadata.FindNavigation(nameof(Notification.Timeline))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}

public class NotificationHistoryConfiguration : IEntityTypeConfiguration<NotificationHistory>
{
    public void Configure(EntityTypeBuilder<NotificationHistory> builder)
    {
        builder.ToTable("NotificationHistories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, value => NotificationHistoryId.Create(value));

        builder.Property(x => x.NotificationId)
            .HasConversion(id => id.Value, value => NotificationId.Create(value))
            .IsRequired();

        builder.Property(x => x.Status).HasConversion<string>();
        builder.Property(x => x.Note).HasMaxLength(1000);
    }
}
