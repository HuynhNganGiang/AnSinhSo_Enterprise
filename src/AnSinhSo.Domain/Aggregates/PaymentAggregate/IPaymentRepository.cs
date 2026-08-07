using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Specifications;

namespace AnSinhSo.Domain.Aggregates.PaymentAggregate;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(PaymentId id, CancellationToken cancellationToken = default);
    Task<Payment?> GetWithDetailsAsync(PaymentId id, CancellationToken cancellationToken = default);
    void Add(Payment payment);
    void Remove(Payment payment);

    // Specification Pattern readiness
    Task<Payment?> FirstOrDefaultAsync(ISpecification<Payment> specification, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Payment>> ListAsync(ISpecification<Payment> specification, CancellationToken cancellationToken = default);
    Task<int> CountAsync(ISpecification<Payment> specification, CancellationToken cancellationToken = default);
}
