using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Application.Abstractions.Persistence;

/// <summary>
/// Giao diện Unit of Work để quản lý transaction.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Lưu các thay đổi vào cơ sở dữ liệu.
    /// </summary>
    /// <param name="cancellationToken">Token hủy yêu cầu.</param>
    /// <returns>Số lượng bản ghi bị ảnh hưởng.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
