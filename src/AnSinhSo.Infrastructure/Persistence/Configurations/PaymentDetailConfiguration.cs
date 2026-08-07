using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.Aggregates.PaymentAggregate.Enumerations;
using AnSinhSo.Domain.Enumerations;
using AnSinhSo.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnSinhSo.Infrastructure.Persistence.Configurations;

public class PaymentDetailConfiguration : IEntityTypeConfiguration<PaymentDetail>
{
    public void Configure(EntityTypeBuilder<PaymentDetail> builder)
    {
        builder.ToTable("PaymentDetails");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasConversion(
                   id => id.Value,
                   value => new PaymentDetailId(value));

        builder.Property(x => x.CitizenId)
               .HasConversion(
                   id => id.Value,
                   value => new CitizenId(value))
               .IsRequired();
               
        builder.HasIndex(x => x.CitizenId);

        builder.Property(x => x.Status)
               .HasConversion(new EnumerationValueConverter<PaymentDetailStatus>())
               .IsRequired();

        builder.OwnsOne(x => x.Amount, money =>
        {
            money.Property(m => m.Amount)
                 .HasColumnName("Amount")
                 .HasColumnType("decimal(18,2)")
                 .IsRequired();

            money.Property(m => m.Currency)
                 .HasColumnName("Currency")
                 .HasConversion(new EnumerationValueConverter<Currency>())
                 .IsRequired();
        });

        builder.Property(x => x.PaidDate);
        
        builder.Property(x => x.FailureReason)
               .HasMaxLength(500);
    }
}
