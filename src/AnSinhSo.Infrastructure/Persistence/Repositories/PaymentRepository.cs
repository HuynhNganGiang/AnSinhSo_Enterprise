using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.Specifications;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public sealed class PaymentRepository : IPaymentRepository
{
    private readonly AnSinhSoDbContext _dbContext;

    public PaymentRepository(AnSinhSoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Payment?> GetByIdAsync(PaymentId id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Payment>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Payment?> GetWithDetailsAsync(PaymentId id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Payment>()
            .Include(p => p.Details)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }


    public void Add(Payment payment)
    {
        _dbContext.Set<Payment>().Add(payment);
    }

    public void Remove(Payment payment)
    {
        _dbContext.Set<Payment>().Remove(payment);
    }

    public Task<Payment?> FirstOrDefaultAsync(ISpecification<Payment> specification, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Payment>().Where(specification.ToExpression()).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Payment>> ListAsync(ISpecification<Payment> specification, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Payment>().Where(specification.ToExpression()).ToListAsync(cancellationToken);
    }

    public Task<int> CountAsync(ISpecification<Payment> specification, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Payment>().Where(specification.ToExpression()).CountAsync(cancellationToken);
    }
}
