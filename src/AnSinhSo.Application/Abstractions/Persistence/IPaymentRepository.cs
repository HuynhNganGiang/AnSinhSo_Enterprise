using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;

namespace AnSinhSo.Application.Abstractions.Persistence;

/// <summary>
/// Giao diện repository cho Payment aggregate root.
/// </summary>
public interface IPaymentRepository : IRepository<Payment, PaymentId>
{
    /// <summary>
    /// Lấy thông tin thanh toán kèm theo chi tiết.
    /// </summary>
    /// <param name="id">Định danh thanh toán.</param>
    /// <param name="cancellationToken">Token hủy yêu cầu.</param>
    /// <returns>Trả về thanh toán kèm chi tiết nếu tìm thấy, ngược lại trả về null.</returns>
    Task<Payment?> GetWithDetailsAsync(PaymentId id, CancellationToken cancellationToken = default);
}
