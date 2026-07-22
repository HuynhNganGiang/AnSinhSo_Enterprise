using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Application.Abstractions.Persistence;

/// <summary>
/// Giao diện generic repository cho các Aggregate Root.
/// </summary>
/// <typeparam name="TEntity">Kiểu của Entity (thường là Aggregate Root).</typeparam>
/// <typeparam name="TId">Kiểu của định danh của Entity.</typeparam>
public interface IRepository<TEntity, in TId>
{
    /// <summary>
    /// Lấy thông tin Entity theo định danh.
    /// </summary>
    /// <param name="id">Định danh của Entity.</param>
    /// <param name="cancellationToken">Token hủy yêu cầu.</param>
    /// <returns>Trả về Entity nếu tìm thấy, ngược lại trả về null.</returns>
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kiểm tra Entity có tồn tại theo định danh hay không.
    /// </summary>
    /// <param name="id">Định danh của Entity.</param>
    /// <param name="cancellationToken">Token hủy yêu cầu.</param>
    /// <returns>Trả về true nếu tồn tại, ngược lại trả về false.</returns>
    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thêm mới một Entity.
    /// </summary>
    /// <param name="entity">Entity cần thêm mới.</param>
    /// <param name="cancellationToken">Token hủy yêu cầu.</param>
    /// <returns>Task đại diện cho tác vụ bất đồng bộ.</returns>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật thông tin Entity.
    /// </summary>
    /// <param name="entity">Entity cần cập nhật.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Xóa một Entity.
    /// </summary>
    /// <param name="entity">Entity cần xóa.</param>
    void Remove(TEntity entity);
}
