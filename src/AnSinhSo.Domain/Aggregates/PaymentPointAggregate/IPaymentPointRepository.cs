using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Domain.Aggregates.PaymentPointAggregate;

public interface IPaymentPointRepository
{
    Task<PaymentPoint?> GetByIdAsync(PaymentPointId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PaymentPoint>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(PaymentPoint paymentPoint);
    void Update(PaymentPoint paymentPoint);
    void Delete(PaymentPoint paymentPoint);
}
