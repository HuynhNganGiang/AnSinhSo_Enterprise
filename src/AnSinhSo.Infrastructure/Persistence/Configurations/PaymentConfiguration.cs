using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AnSinhSo.Infrastructure.Persistence.Converters;
using AnSinhSo.Domain.Aggregates.PaymentAggregate.Enumerations;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasConversion(
                   id => id.Value,
                   value => new PaymentId(value));

        builder.Property(x => x.PolicyId)
               .HasConversion(
                   id => id.Value,
                   value => new AnSinhSo.Domain.Aggregates.PolicyAggregate.PolicyId(value))
               .IsRequired();
               
        builder.HasIndex(x => x.PolicyId);

        builder.Property(x => x.Status)
               .HasConversion(new EnumerationValueConverter<PaymentStatus>())
               .IsRequired();

        builder.HasMany(x => x.Details)
               .WithOne()
               .HasForeignKey("PaymentId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Details)
               .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
