using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, value => new PaymentId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.PaymentNumber)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasIndex(x => x.PaymentNumber).IsUnique();

        builder.Property(x => x.CitizenId)
            .HasConversion(id => id.Value, value => new CitizenId(value))
            .IsRequired();

        builder.Property(x => x.HouseholdId)
            .HasConversion(id => id != null ? id.Value : (System.Guid?)null, value => value.HasValue ? new HouseholdId(value.Value) : null);

        builder.Property(x => x.WelfareCaseId)
            .HasConversion(id => id.Value, value => new WelfareCaseId(value))
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.ScheduledDate)
            .IsRequired();

        builder.Property(x => x.ActualPaymentDate);

        builder.Property(x => x.Method)
            .HasConversion(method => method.Id, value => PaymentMethod.FromId(value))
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion(status => status.Id, value => PaymentStatus.FromId(value))
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.HasOne<Citizen>()
            .WithMany()
            .HasForeignKey(x => x.CitizenId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<WelfareCase>()
            .WithMany()
            .HasForeignKey(x => x.WelfareCaseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.CitizenId);
        builder.HasIndex(x => x.HouseholdId);
        builder.HasIndex(x => x.WelfareCaseId);
    }
}
