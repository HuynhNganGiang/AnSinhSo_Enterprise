using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.PaymentPointAggregate;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public class PaymentPointRepository : IPaymentPointRepository
{
    private readonly AnSinhSoDbContext _context;

    public PaymentPointRepository(AnSinhSoDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentPoint?> GetByIdAsync(PaymentPointId id, CancellationToken cancellationToken = default)
    {
        return await _context.PaymentPoints
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<PaymentPoint>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PaymentPoints
            .ToListAsync(cancellationToken);
    }

    public void Add(PaymentPoint paymentPoint)
    {
        _context.PaymentPoints.Add(paymentPoint);
    }

    public void Update(PaymentPoint paymentPoint)
    {
        _context.PaymentPoints.Update(paymentPoint);
    }

    public void Delete(PaymentPoint paymentPoint)
    {
        _context.PaymentPoints.Remove(paymentPoint);
    }
}
