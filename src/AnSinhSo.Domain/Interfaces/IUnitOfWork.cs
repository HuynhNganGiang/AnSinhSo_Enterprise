using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Domain.Interfaces;

/// <summary>
/// Hợp đồng chung cho Unit of Work Pattern, đảm bảo tính nhất quán của dữ liệu (Transaction).
/// Mọi thay đổi dữ liệu trên Repository chỉ được lưu xuống hệ thống lưu trữ thực sự khi gọi SaveChangesAsync.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Lưu tất cả thay đổi trên bối cảnh hiện tại xuống cơ sở dữ liệu.
    /// </summary>
    /// <param name="cancellationToken">Token hủy tác vụ bất đồng bộ.</param>
    /// <returns>Số lượng bản ghi bị ảnh hưởng.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    void ClearChangeTracker();
}
