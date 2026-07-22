using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.PolicyAggregate;

namespace AnSinhSo.Application.Abstractions.Persistence;

/// <summary>
/// Giao diện repository cho Policy aggregate root.
/// </summary>
public interface IPolicyRepository : IRepository<Policy, PolicyId>
{
    /// <summary>
    /// Lấy danh sách các chính sách đang có hiệu lực.
    /// </summary>
    /// <param name="cancellationToken">Token hủy yêu cầu.</param>
    /// <returns>Trả về danh sách các chính sách đang hoạt động.</returns>
    Task<IEnumerable<Policy>> GetActiveAsync(CancellationToken cancellationToken = default);
}
