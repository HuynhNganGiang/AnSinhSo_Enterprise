using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Infrastructure.Persistence.Contexts;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public class PaymentRepository : Repository<Payment, PaymentId>, IPaymentRepository
{
    public PaymentRepository(AnSinhSoDbContext dbContext) : base(dbContext)
    {
    }

    public Task<Payment?> GetWithDetailsAsync(PaymentId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(PaymentId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Update(Payment entity)
    {
        throw new NotImplementedException();
    }
}
