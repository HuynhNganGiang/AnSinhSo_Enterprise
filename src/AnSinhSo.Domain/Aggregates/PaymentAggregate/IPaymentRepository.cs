using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Domain.Aggregates.PaymentAggregate;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(PaymentId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(PaymentId id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Payment> Items, int TotalCount)> SearchAsync(
        string? keyword,
        Guid? citizenId,
        Guid? householdId,
        Guid? welfareCaseId,
        int? statusId,
        int? methodId,
        DateTime? fromDate,
        DateTime? toDate,
        int page,
        int pageSize,
        string? sort,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Payment>> GetPendingPaymentsAsync(CancellationToken cancellationToken = default);
    void Add(Payment payment);
    void Update(Payment payment);
}
